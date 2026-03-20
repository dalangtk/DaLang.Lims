@using DaLang.Lims.Web.Dev;
@using DaLang.Lims.Web.DynamicApi.Enums;
@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    if (gen.Fields == null) return;
    if (gen.Fields.Count() == 0) return;

    var areaName = "" + gen.ApiAreaName;
    var entityName = "" + gen.EntityName;
    var entityNameLower = entityName.ToLower();

    var areaNamePc = areaName.NamingPascalCase();
    var entityNamePc = entityName.NamingPascalCase();

    var areaNameKc = areaName.NamingKebabCase();// KebabCase(areaName);
    var entityNameKc = entityName.NamingKebabCase();// KebabCase(entityName);

    var areaNameCc = areaName.NamingCamelCase();// camelCase(areaName);
    var entityNameCc = entityName.NamingCamelCase();// camelCase(entityName);
    var areaGrouping = "";
    if(!string.IsNullOrWhiteSpace(gen.AreaGrouping)){
        areaGrouping = "/" + gen.AreaGrouping.ToLower();
    }

    var at = "@";
    var apiName = entityName + "Api";

    var permissionArea = string.Concat(areaNameKc, ":", entityNameKc.ToLower());

    var queryColumns = gen.Fields.Where(w => w.WhetherQuery);

    var defineUiComponentsImportPath = new Dictionary<string, string>()
    {
        //{"my-select-dictionary","@/components/my-select-window/dictionary" },
        //{"my-role","@/components/my-select-window/role" },
        //{"my-user","@/components/my-select-window/user"},
        //{"my-position","@/components/my-select-window/position" }
        //{"my-upload","@/components/my-upload/index" }
    };
    var uiComponentsMethodName = new Dictionary<string, string>()
    {
        //{"my-select-dictionary","onOpenDic" },
        //{"my-role","onOpenRole" },
        //{"my-user","onOpenUser" },
        //{"my-position","onOpenPosition" }
    };
    var editors = gen.Fields.Select(s => s.Editor).Distinct();
    var uiComponentsInfo = editors
        .Where(w => defineUiComponentsImportPath.Keys.Contains(w))
        .Select(s => new { ImportName = s.NamingPascalCase(), ImportPath = defineUiComponentsImportPath[s] });

    // 获取数据输入控件
    string editorName(DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenFieldEntity col, out string attrs, out string innerBody,out string subfix,out string colWidth)
    {
        attrs = string.Empty;
        subfix = string.Empty;
        innerBody = string.Empty;
        colWidth= new List<string>(){"my-upload","my-editor","my-input-textarea"}.Contains(col.Editor)?"24":"12";
        var editorName = col.Editor;
        if (String.IsNullOrWhiteSpace(editorName)) editorName = "el-input";
        if (!string.IsNullOrWhiteSpace(col.DictTypeCode))
        {
            editorName = "el-select";
            if( col.IsNullable)attrs += " clearable ";
            innerBody = string.Concat("<el-option v-for=", "\"item in state.dicts['", col.DictTypeCode, "']\" :key=\"item.value\" :value=\"item.value\" :label=\"item.name\" />");
        }
        else if (col.Editor == "el-date-picker"){
            editorName = "el-date-picker";
            attrs += " value-format=\"YYYY-MM-DD\"";
            if( col.IsNullable)attrs += " clearable";
        }
        else if (col.Editor == "el-select")
        {
            editorName = col.Editor;
            if (col.IsNullable) attrs += " clearable ";
            if(!String.IsNullOrWhiteSpace(col.DisplayColumn) || !String.IsNullOrWhiteSpace(col.ValueColumn))
            {
                var labels = (""+col.DisplayColumn).Split(',');
                var values = ("" + col.ValueColumn).Split(',');
                var cout = labels.Length;
                if (values.Length > cout) cout = values.Length;
                for(var i = 0; i < cout; i++)
                {
                    innerBody += string.Concat("<el-option value=\"" + (values.Length > i ? values[i] : "") + "\" label=\"" + (labels.Length > i ? labels[i] : "") + "\" />");
                }
            }
        }
        else if(defineUiComponentsImportPath.Keys.Any(a => a == col.Editor))
        {
            attrs = attrs + " class=\"input-with-select\" ";
            innerBody = "<el-button slot=\"append\" icon=\"el-icon-more\" @click=\"" + uiComponentsMethodName[col.Editor] + "('editForm','" + col.DictTypeCode + "','" + col.Title + "')\" />";
        }
        else if (col.Editor == "my-upload")
        {
            editorName = "my-upload";
            attrs += " v-if='state.showDialog' ";
        }
        else if (col.Editor == "my-editor")
        {
            editorName = "my-editor";
            attrs += " v-if='state.showDialog' ";
        }
        else if (col.Editor == "my-input-textarea"){
            editorName= "el-input";
            attrs += " type=\"textarea\" ";
        }
        else if (col.Editor == "my-input-number"){
            editorName= "el-input";
            attrs += " type=\"number\" ";
        }
        else if (col.Editor == "my-bussiness-select"){
            editorName= "el-select";
            if (col.IsNullable) attrs += " clearable ";
            if (col.IncludeMode == 1){
                attrs += " multiple ";
                subfix="_Values";
            }
            if(!String.IsNullOrWhiteSpace(col.IncludeEntity)){
                //业务下拉前缀
                var selectPrefix = col.IncludeEntity.Replace("Entity", "");
                var selectTitle="name";
                if(!String.IsNullOrWhiteSpace(col.IncludeEntityKey))
                    selectTitle=col.IncludeEntityKey.NamingCamelCase();
                if (col.IncludeMode == 1){
                    //一对多,转换模型 xxxIds_Values
                    innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"String(item.id)\" :label=\"item.",selectTitle,"\" />");
                }else{
                    innerBody = string.Concat("<el-option v-for=", "\"item in state.select",selectPrefix,"ListData\" :key=\"item.id\" :value=\"item.id\" :label=\"item.",selectTitle,"\" />");
                }
            }
        }

        return editorName;
    }
    
    var dictCodes = gen.Fields.Where(w => !String.IsNullOrWhiteSpace(w.DictTypeCode)).Select(s => s.DictTypeCode).Distinct();// editors.Any(a => a == "my-select-dictionary");
    var hasDict = dictCodes.Any();
    //关联的模型
    var includeFieldEntitys = gen.Fields.Where(w => !String.IsNullOrWhiteSpace(w.IncludeEntity)).Select(w=>w.IncludeEntity.Replace("Entity", "")).Distinct();
    var hasUpload=editors.Any(a=>a=="my-upload");
    //var hasRole = editors.Any(a => a == "my-role");
    //var hasUser = editors.Any(a => a == "my-user");
    //var hasPosition = editors.Any(a => a == "my-position");

    string jsBool(Boolean exp){
        return exp ? "true" : "false";
    }
}
@{ 
    string attributes, inner, subfix,colWidth;
}
<template>
<div class="my-layout my-container">
    <TableSearch :search="state.search" @(at)search="onSearch" />
    <MyTable border tableName="baseGroup" class="my-table" :data="state.@(entityNameCc)ListData" ref="table" :total="state.total"
      v-on:pageOrSizeChange="onTablePageOrSizeChange" :loading="state.loading" stripe>
      <template #headerButton>
        <el-button v-if="auth(perms.add)" type="primary" size="small" @(at)click="onAdd">
        <SvgIcon name="ele-Plus" />
        新增</el-button>
      </template>
        @foreach (var col in gen.Fields.Where(w => w.WhetherTable && !w.IsIgnoreColumn()))
        {
            if(col.ColumnName == "IsValid")
                continue;
            if(col.IsIncludeColumn()&&!string.IsNullOrWhiteSpace(col.IncludeEntityKey))
            {
                if(col.IncludeMode==0)
                {
                   @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())_Text" label="@(col.Title)" show-overflow-tooltip width />
                }
                else if(col.IncludeMode==1)
                {  
                   @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())_Texts" label="@(col.Title)" show-overflow-tooltip width >
                   @:  <template #default="{ row }">
                   @:    {{ row.@(col.ColumnName.NamingCamelCase())_Texts ? row.@(col.ColumnName.NamingCamelCase())_Texts.join(',') : '' }}
                   @:  </template>
                   @:</el-table-column>
                }
            }
            else if(col.Editor=="my-upload")
            {
                  @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())" label="@(col.Title)" show-overflow-tooltip width >
                  @:  <template #default="{ row }">
                  @:   <div class="my-flex">
                  @:     <el-image :src="row.@(col.ColumnName.NamingCamelCase())" :preview-src-list="preview@(col.ColumnName)list"
                  @:       :initial-index="get@(col.ColumnName)InitialIndex(row.@(col.ColumnName.NamingCamelCase()))" :lazy="true" :hide-on-click-modal="true" fit="scale-down"
                  @:       preview-teleported style="width: 80px; height: 80px" />
                  @:     <div class="ml10 my-flex-fill my-flex-y-center">
                  @:     </div>
                  @:   </div>
                  @: </template>
                  @:</el-table-column>
            }
            else
            {
                  @:<el-table-column prop="@(col.ColumnName.NamingCamelCase())@if(!string.IsNullOrWhiteSpace(col.DictTypeCode))@("DictName")" label="@(col.Title)" show-overflow-tooltip width />
            }
        }
      <el-table-column prop="isValid" label="状态" min-width="100">
        <template #default="{ row }">
          <el-tag :type="row.isValid === true ? 'success' : 'warning'">
            {{ row.isValid === true ? "启用" : "停用" }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column v-auths="[perms.delete]" label="操作" :width="actionColWidth" fixed="right">
        <template #default="{ row }">
          <el-button v-auth="perms.update" icon="ele-EditPen" size="small" text type="primary"
            @(at)click="onEdit(row)">编辑</el-button>
          <el-button text type="danger" v-if="auth(perms.delete)" @(at)click="onDelete(row)"
            icon="ele-Delete">删除</el-button>
        </template>
      </el-table-column>
    </MyTable>
    
    <@(entityNameKc)-form ref="@(entityNameCc)FormRef" :title="state.@(entityNameCc)FormTitle"></@(entityNameKc)-form>
  </div>
</template>

<script lang="ts" setup name="@(areaNameCc)/@(entityNameLower)">
import { ref, reactive, onMounted, getCurrentInstance, onBeforeMount, defineAsyncComponent, computed } from 'vue'
import TableSearch from '/@(at)/components/my-table/MyTableSearch.vue'
import MyTable from '/@(at)/components/my-table/index.vue'
import { GetPageInput } from '/@(at)/api/@(areaNameCc)@(areaGrouping)/datacontract/base'
import {
  @(entityNamePc)Output,
  @(entityNamePc)ListOutput,
  @(entityNamePc)QueryListInput
}
  from '/@(at)/api/@(areaNameCc)@(areaGrouping)/datacontract/@(entityNameLower)-datacontract'

@if(gen.Fields.Any(s=>s.Editor=="my-upload")){
@:import {  FileGetPageOutput } from '/@(at)/api/admin/data-contracts'
}
import { @(apiName) } from '/@(at)/api/@(areaNameKc)@(areaGrouping)/@(entityNameLower)'
@if (includeFieldEntitys.Any())
{
    foreach(var incField in includeFieldEntitys)
    {
@:import { @(incField)Api } from '/@(at)/api/@(areaNameKc)/@(incField)'
    }
}
@if (hasDict)
{
@:import { DictApi } from '/@(at)/api/admin/Dict'
}
import eventBus from '/@(at)/utils/mitt'
import { auth, auths, authAll } from '/@(at)/utils/authFunction'

// 引入组件
const @(entityNamePc)Form = defineAsyncComponent(() => import('./components/@(entityNameLower)-form.vue'))

const { proxy } = getCurrentInstance() as any
var table = ref();
const @(entityNameCc)FormRef = ref()

//权限配置
const perms = {
  add:'api:@(permissionArea):add',
  update:'api:@(permissionArea):update',
  delete:'api:@(permissionArea):delete',
  batDelete:'api:@(permissionArea):batch-delete',
}

const actionColWidth = authAll([perms.update, perms.softDelete]) || authAll([perms.update, perms.delete]) ? 135 : 70

const state = reactive({
  loading: false,
  @(entityNameCc)FormTitle: '',
  total: 0,
  search: [
      @if(queryColumns.Any()){
          foreach(var c in queryColumns){
@:{ label: @("'"+(@c.Title)+"'"), prop: @("'"+@c.ColumnName.NamingCamelCase()+"'"), placeholder: @("'"+(@c.Title)+"'"), required: false, type: 'input' },
          }
      }
    
  ],
  sels: [] as Array<@(entityNamePc)Output>,
  filter: {
@foreach(var f in queryColumns.Where(w=>!w.IsIgnoreColumn())){
@:    @(f.ColumnName.NamingCamelCase()): null,
}
  } as @(entityNamePc)QueryListInput,
  pageInput: {
    currentPage: 1,
    pageSize: 20,
    filter:{

    }
  } as GetPageInput<@(entityNamePc)QueryListInput>,
  @(entityNameCc)ListData: [] as Array<@(entityNamePc)Output>,
  @foreach(var incField in includeFieldEntitys){
@:  select@(incField)ListData: [] as @(incField)Output[],
}
@foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:  file@(col.ColumnName)ListData: [] as Array<FileGetPageOutput>,
}
@if (hasDict){
  @://字典相关
  @:dicts:{
    foreach (var d in dictCodes)
    {
    @:"@(d)":[],   
    }
  @:}
    }
})

onMounted(() => {

@foreach(var incField in includeFieldEntitys){
@:  get@(incField)List();
}
    @if (hasDict)
    {
@:  getDictsTree()      
    }
  onQuery()
  eventBus.off('refresh@(entityNamePc)')
  eventBus.on('refresh@(entityNamePc)', async () => {
    onQuery()
  })
})

onBeforeMount(() => {
  eventBus.off('refresh@(entityNamePc)')
})

const onSearch = (data: EmptyObjectType) => {
  state.pageInput.filter = Object.assign({}, state.pageInput.filter, { ...data });
  table.value.pageReset();
}

@foreach(var incField in includeFieldEntitys){
@:const get@(incField)List = async () => {
@:  const res = await new @(incField)Api().getList({}).catch(() => {
@:    state.select@(incField)ListData = []
@:  })
@:  state.select@(incField)ListData = res?.data || []
@:}
}

@if (hasDict)
{
@://获取需要使用的字典树
@:const getDictsTree = async () => {
@:  let res = await new DictApi().getList(['@(string.Join("','", dictCodes))'])
@:  if(!res?.success)return;
@:    state.dicts = res.data
@:}
}

const onQuery = async () => {
  state.loading = true
  //state.pageInput.filter = state.filter
  const res = await new @(apiName)().getPage(state.pageInput).catch(() => {
    state.loading = false
  })

  state.@(entityNameCc)ListData = res?.data?.list ?? []
  state.total = res?.data?.total ?? 0
  state.loading = false

  @foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:  state.file@(col.ColumnName)ListData = res?.data?.list?.map(s => {
@:    return { linkUrl: s.@(col.ColumnName.NamingCamelCase()) }
@:  }) ?? []
}
}

const onAdd = () => {
  state.@(entityNameCc)FormTitle = '新增@(gen.BusName)'
  @(entityNameCc)FormRef.value.open()
}

const onEdit = (row: @(entityNamePc)Output) => {
  state.@(entityNameCc)FormTitle = '编辑@(gen.BusName)'
  @(entityNameCc)FormRef.value.open(row)
}

const onDelete = (row: @(entityNamePc)Output) => {
  proxy.$modal
    .confirmDelete(`确定要删除【${row.name}】?`)
    .then(async () => {
      await new @(apiName)().delete({ id: row.id }, { loading: true, showSuccessMessage: true })
      onQuery()
    })
    .catch(() => {})
}

const onTablePageOrSizeChange = async (page: TablePageType) => {
  state.pageInput.currentPage = page.currentPage;
  state.pageInput.pageSize = page.pageSize;
  await onQuery();
}

@if(gen.GenBatchDelete){
@:const onBatchDelete = async () => {
@:  proxy.$modal?.confirmDelete(`确定要删除选择的${state.sels.length}条记录？`).then(async () =>{
@:    const rst = await new @(apiName)().batchDelete(state.sels?.map(item=>item.id) as number[], { loading: true, showSuccessMessage: true })
@:    if(rst?.success){
@:      onQuery()
@:    }
@:  })
@:}
}

@foreach (var col in gen.Fields.Where(s=>s.Editor=="my-upload")){
@:const preview@(col.ColumnName)list = computed(() => {
@:  let imgList = [] as string[]
@:  state.file@(col.ColumnName)ListData.forEach((a) => {
@:    if (a.linkUrl) {
@:      imgList.push(a.linkUrl as string)
@:    }
@:  })
@:  return imgList
@:})
@:const get@(col.ColumnName)InitialIndex = (imgUrl: string) => {
@:  return preview@(col.ColumnName)list.value.indexOf(imgUrl)
@:}
}
</script>
<style scoped>
.my-container {
  padding-top: 10px;
}

.my-table {
  flex: 1;
  overflow: hidden;
}
</style>