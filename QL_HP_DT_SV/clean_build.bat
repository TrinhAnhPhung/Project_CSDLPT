@echo off
echo Cleaning build folders...
if exist "bin\Debug" rmdir /s /q "bin\Debug"
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "obj\Debug" rmdir /s /q "obj\Debug"
if exist "obj\Release" rmdir /s /q "obj\Release"
echo Done!

