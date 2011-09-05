@echo off
@echo.
@echo -----------------------------------------------------
@echo -----------------------------------------------------
@echo - 
@echo - This process will build Xen 2.0 for XNA
@echo - 
@echo - A VisualStudio CustomTool plugin will be installed,
@echo - which may prompt for administration rights
@echo -
@echo -----------------------------------------------------
@echo -----------------------------------------------------
@echo.
pause
@echo.
@echo Building xen for XNA GameStudio 4.0
@echo This may take a minute or so...
@echo.

@echo Building xen Shader System
@echo.

%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /p:Configuration=Release /p:Optimize=true /p:DebugSymbols=false /verbosity:minimal .\xen\prebuild\sln\build_ss.sln
@echo.

@echo Register Visual Studio Custom Tool
"xen\bin\Xen.Graphics.ShaderSystem\registry\XenFxRegistryTest.exe" "xen\bin\Xen.Graphics.ShaderSystem\registry\XenFxRegistrySetup.exe" "xen\bin\Xen.Graphics.ShaderSystem\Xen.Graphics.ShaderSystem.CustomTool.dll" "XenFX" "{B52AC702-3C02-4128-A247-92E766C258C9}"
@echo Building Xen.Ex Shaders
@echo Building Xen.Ex Filters FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Filters\Shader.fx" "Xen.Ex.Filters" "xen\src\Xen.Ex\Filters\Shader.fx.cs"
@echo Building Xen.Ex Graphics2D FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics2D\FillTex.fx" "Xen.Ex.Graphics2D" "xen\src\Xen.Ex\Graphics2D\FillTex.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics2D\Shader.fx" "Xen.Ex.Graphics2D" "xen\src\Xen.Ex\Graphics2D\Shader.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics2D\statistics\Shader.fx" "Xen.Ex.Graphics2D.Statistics" "xen\src\Xen.Ex\Graphics2D\Statistics\Shader.fx.cs"
@echo Building Xen.Ex Shaders FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Shaders\Simple.fx" "Xen.Ex.Shaders" "xen\src\Xen.Ex\Shaders\Simple.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Shaders\Depth.fx" "Xen.Ex.Shaders" "xen\src\Xen.Ex\Shaders\Depth.fx.cs"
@echo Building Xen.Ex Material FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Material\Material.fx" "Xen.Ex.Material" "xen\src\Xen.Ex\Material\Material.fx.cs"

@echo Building Xen.Ex Gpu Particle Core FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics\Processor\GpuParticles.fx" "Xen.Ex.Graphics.Processor" "xen\src\Xen.Ex\Graphics\Processor\GpuParticles.fx.cs"

@echo Building Xen.Ex Gpu Particle 2D Display FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics\Display\Billboard.fx" "Xen.Ex.Graphics.Display" "xen\src\Xen.Ex\Graphics\Display\Billboard.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics\Display\VelocityBillboard.fx" "Xen.Ex.Graphics.Display" "xen\src\Xen.Ex\Graphics\Display\VelocityBillboard.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics\Display\VelocityLines.fx" "Xen.Ex.Graphics.Display" "xen\src\Xen.Ex\Graphics\Display\VelocityLines.fx.cs"
@echo Building Xen.Ex Gpu Particle 3D Display FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics\Display\Billboard3D.fx" "Xen.Ex.Graphics.Display" "xen\src\Xen.Ex\Graphics\Display\Billboard3D.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Ex\Graphics\Display\VelocityBillboard3D.fx" "Xen.Ex.Graphics.Display" "xen\src\Xen.Ex\Graphics\Display\VelocityBillboard3D.fx.cs"

@echo Building Logo FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Xen.Logo\VelocityBillboard.fx" "Xen.Logo" "xen\src\Xen.Logo\VelocityBillboard.fx.cs"

@echo Building Tutorials.Tutorial_03 FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 03\shader.fx" "Tutorials.Tutorial_03" "xen\src\Tutorials\Tutorial 03\shader.fx.cs"
@echo Building Tutorials.Tutorial_09 FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 09\shader.fx" "Tutorials.Tutorial_09" "xen\src\Tutorials\Tutorial 09\shader.fx.cs"
@echo Building Tutorials.Tutorial_16 FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 16\shader.fx" "Tutorials.Tutorial_16" "xen\src\Tutorials\Tutorial 16\shader.fx.cs"
@echo Building Tutorials.Tutorial_25 FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 25\shader.fx" "Tutorials.Tutorial_25" "xen\src\Tutorials\Tutorial 25\shader.fx.cs"
@echo Building Tutorials.Tutorial_28 FX
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 28\Shaders\AlphaOutput.fx" "Tutorials.Tutorial_28.Shaders" "xen\src\Tutorials\Tutorial 28\Shaders\AlphaOutput.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 28\Shaders\Background.fx" "Tutorials.Tutorial_28.Shaders" "xen\src\Tutorials\Tutorial 28\Shaders\Background.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 28\Shaders\Character.fx" "Tutorials.Tutorial_28.Shaders" "xen\src\Tutorials\Tutorial 28\Shaders\Character.fx.cs"
xen\bin\Xen.Graphics.ShaderSystem\cmdxenfx.exe "xen\src\Tutorials\Tutorial 28\Shaders\Composite.fx" "Tutorials.Tutorial_28.Shaders" "xen\src\Tutorials\Tutorial 28\Shaders\Composite.fx.cs"
@echo Building Xen and Xen.Ex DEBUG
%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe /p:Configuration=Debug /t:Rebuild .\xen\prebuild\sln\prebuild.sln
@echo.
@echo -----------------------------------
@echo -----------------------------------
@echo - If part of the prebuild failed,
@echo - Check the following are installed:
@echo -
@echo - .NET Framework 4.0
@echo - XNA Game Studio 4.0
@echo - DirectX SDK (sometimes required)
@echo -----------------------------------
@echo -----------------------------------
@echo.
@echo -----------------------------------
@echo - To get started, open:
@echo - ./xen/Tutorials.sln
@echo -----------------------------------
@echo.
pause