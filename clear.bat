@echo off
setlocal enabledelayedexpansion

REM 设置要扫描的根目录
set ROOT_DIR=%cd%

REM 递归遍历目录
for /R "%ROOT_DIR%" %%f in (.) do (
    REM 获取当前目录名
    set CURRENT_DIR=%%~nf
    if "!CURRENT_DIR!" == "bin" (
        echo Deleting directory: %%f
        rd /s /q "%%f"
    ) else if "!CURRENT_DIR!" == "obj" (
        echo Deleting directory: %%f
        rd /s /q "%%f"
    )
)

echo Finished.
pause