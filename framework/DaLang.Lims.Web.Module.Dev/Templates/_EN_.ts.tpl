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

    var queryColumns = gen.Fields.Where(w => w.WhetherQuery);
    var commonFields = new String[] { "id", "ProId", "ProName", "ProTime", "ModId"
    , "ModName","ModTime","IsModified", "IsDeleted","TenantId"};
}

import { ContentType, HttpClient, RequestParams } from '/@(at)/api/admin/http-client'
import { AxiosResponse } from 'axios'
import {
  @(entityNamePc)Output,
  @(entityNamePc)ListOutput,
  @(entityNamePc)AddInput,
  @(entityNamePc)UpdateInput,
  @(entityNamePc)QueryListInput,
} from './datacontract/@(entityNameLower)-datacontract'
import { ResultBaseOutput, ResultBasePageOutput, GetPageInput } from '/@(at)/api/lims/basedata/datacontract/base'

export class @(entityNamePc)Api<SecurityDataType = unknown> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @(at)tags @(areaNameCc)
   * @(at)name GetPage
   * @(at)summary 查询分页
   * @(at)request POST:/api/@(areaName)/@(entityNameKc.ToLower())/get-page
   * @(at)secure
   */
  getPage = (data: GetPageInput<@(entityNamePc)QueryListInput>, params: RequestParams = {}) =>
    this.request<ResultBasePageOutput<@(entityNamePc)ListOutput>, any>({
      path: `/api/@(areaName)/@(entityNameKc.ToLower())/get-page`,
      method: 'POST',
      body: data,
      secure: true,
      type: ContentType.Json,
      format: 'json',
      ...params,
    })
    /**
   * No description
   *
   * @(at)tags @(areaNameCc)
   * @(at)name Get
   * @(at)summary 查询@(gen.BusName)
   * @(at)request GET:/api/@(areaName)/@(entityNameKc.ToLower())/get
   * @(at)secure
   */
  get = (
    query?: {
      /** @(at)format int64 */
      id?: number
    },
    params: RequestParams = {}
  ) =>
    this.request<ResultBaseOutput<@(entityNamePc)Output>, any>({
      path: `/api/@(areaName)/@(entityNameKc.ToLower())/get`,
      method: 'GET',
      query: query,
      secure: true,
      format: 'json',
      ...params,
    })
  /**
  * No description
  *
  * @(at)tags @(areaNameCc)
  * @(at)name GetAll
  * @(at)summary 查询@(gen.BusName)所有数据
  * @(at)request GET:/api/@(areaName)/@(entityNameKc.ToLower())/getAll
  * @(at)secure
  */
 getAll = () =>
   this.request<ResultBaseOutput<Array<@(entityNamePc)Output>>, any>({
     path: `/api/@(areaName)/@(entityNameKc.ToLower())/get-all`,
     method: 'GET',
     secure: true,
     format: 'json',
   })
 /**
  * No description
  *
  * @(at)tags @(areaNameCc)
  * @(at)name Add
  * @(at)summary 新增@(gen.BusName)
  * @(at)request POST:/api/@(areaName)/@(entityNameKc.ToLower())/add
  * @(at)secure
  */
 add = (data: @(entityNamePc)AddInput, params: RequestParams = {}) =>
   this.request<ResultBaseOutput<number>, any>({
     path: `/api/@(areaName)/@(entityNameKc.ToLower())/add`,
     method: 'POST',
     body: data,
     secure: true,
     type: ContentType.Json,
     format: 'json',
     ...params,
   })
 /**
  * No description
  *
  * @(at)tags @(areaNameCc)
  * @(at)name Update
  * @(at)summary 修改@(gen.BusName)
  * @(at)request PUT:/api/@(areaName)/@(entityNameKc.ToLower())/update
  * @(at)secure
  */
 update = (data: @(entityNamePc)UpdateInput, params: RequestParams = {}) =>
   this.request<AxiosResponse, any>({
     path: `/api/@(areaName)/@(entityNameKc.ToLower())/update`,
     method: 'PUT',
     body: data,
     secure: true,
     type: ContentType.Json,
     ...params,
   })
 /**
  * No description
  *
  * @(at)tags @(areaNameCc)
  * @(at)name Delete
  * @(at)summary 删除@(gen.BusName)
  * @(at)request DELETE:/api/@(areaName)/@(entityNameKc.ToLower())/delete
  * @(at)secure
  */
 delete = (
   query?: {
     /** @(at)format int64 */
     id?: number
   },
   params: RequestParams = {}
 ) =>
   this.request<AxiosResponse, any>({
     path: `/api/@(areaName)/@(entityNameKc.ToLower())/delete`,
     method: 'DELETE',
     query: query,
     secure: true,
     ...params,
   })
}
