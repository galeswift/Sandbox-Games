Xen: Graphics API for XNA
www.codeplex.com/xen

version 2.0 (XNA 4.0)




-- Quick Start:

1.) Run 'build' batch file in the root directory!
2.) Open ./xen/Tutorials.sln


Tutorials.sln contains 29 tutorial applications.
Reading these will make learning xen a lot easier!

Full Xen projects for PC and Xbox are located in ./xen/src/









-----------------------------------------------------------------------------------------------------
-- Copyright & Contact


Xen is Copyright (C) 2008-2011 Graham Aldridge
graham.aldridge@gmail.com
XboxLive: StatusUnknown



-----------------------------------------------------------------------------------------------------
-- Change Log

----------------
-- 22/FEB/11
-- Version 2.0

2.0 is a major breaking upgrade to the API
Almost all parts of the API have been modified in some way
Xen 2.0 is NOT backwards compatibile with 1.x!

XNA 4 is now required, 3.1 is not supported
Animation and Instancing are now first class citizens in the API
The shader system has been radically overhauled internally
Shaders now have animation and instancing support *automatically generated*
The DrawState API has changed radically, with distinct stacks and 'using' support
The FrameState and ContentState objects are introduced for strictness reasons
MaterialShader has been rebuilt from the ground up for XNA 4 compatibility
There have been many bug fixes, tweaks and improvements throughout
The xbox now has a menu for selecting tutorials
The 'update FX' tool is added, to automatically update all shaders it finds

----------------
-- 10/APR/10
-- Version 1.8.1

[Fixed] The HDR tutorial now uses Power-of-Two cubemaps that are separated into RGB and M textures

----------------
-- 5/APR/10
-- Version 1.8

This update fixes a number of bugs in several areas of the API and introduces a large new Tutorial.

[Added] L2 Spherical Harmonic class added to Xen.Ex
[Added] Tutorial 28: HDR Lighting added: This is a massive tutorial, covering many areas of lighting
[Updated] Simplified the Shadow Mapping tutorial
[Updated] (Breaking change) The blur filter now requires specifying a blur exponent. A value of 1.0f maintains old behavior
[Updated] The shader system now supports arrays marked 'GLOBAL'
[Fixed] Several issues with animation caching behavior
[Fixed] Several issues with the shader system error reporting
[Fixed] Rare preshader initializations issues in the shader system
[Fixed] A subtle bug where the XNA shader compiler for the 360 would sometimes ignore swizzles (and not tell you!)

----------------
-- 15/JAN/10
-- Version 1.7.1

[Fixed] A number of small bugs have been fixed

----------------
-- 01/NOV/09
-- Version 1.7

This update is a long time coming. Thanks everyone for your patience during my move!

[Updated] Xen is now updated to XNA 3.1 by default. (Xen is backwards compatible right back to 2.0 with compiler flags)
[Added] Avatar support, using the same animation system as ModelInstance.
[Added] Two tutorials, Avatars and Threading techniques
[Added] Shader System support for SM3 boolean constants (preshaders included)
[Added] XnaGameComponentHost - host a Xen application within an XNA Game Component (BETA)
[Added] XenLogo - An external class that displays an animated Xen Startup Logo
[Fixed] A number of bugs, large and small have been fixed in this realease. Especially in the shader system.

----------------
-- 11/JUN/09
-- Version 1.6.3

[Added] DrawDynamicVertices and DrawDynamicIndexedVertices methods in the DrawState object. These methods wrap XNA DrawUserPrimitves and DrawUserIndexedPrimitves, however they deal with the VertexDeclaration for you and keep track of state.
[Added] A shader Technique can now be given a string attribute 'BaseTypes' which specify a comma delimited list of type names to implement (Thanks Darren!). With this, you can define custom interfaces for your shaders to implement.
[Fixed] A bug with Dispose ordering in the ContentRegister
[Fixed] A number of small bugs to do with integer shader values, as 99.9% of the time they are processed as floats
[Fixed] Missed detection of certain keywords in a shader, such as 'VertexShader' which are not case sensitive

----------------
-- 29/MAY/09
-- Version 1.6.2 (Cumulative updates to the shader processor)

[Fixed] The shader compiler now skips ASM extraction for include header files, preventing a 'No Techniques' exception in the HLSL FX Compiler
[Fixed] A small bug in the shader compiler where line splitting would wrongly remove empty lines
[Fixed] An issue where an include handler wasn't always being used with CompileFromSource() shader compiling
[Fixed] An issue where techniques that had annotations would be ignored by the shader compiler
[Fixed] Two issues where partial precision would cause errors with Xbox shaders
[Fixed] A bug with Preshader constant extraction
[Fixed] Added support for some basic flow control instructions in the Xbox shader HLSL processor

----------------
-- 25/MAY/09
-- Version 1.6.1 (Minor bug fix release)

[Fixed] 2D ElementRect ClipChildren is now more robust
[Fixed] DrawStatisticsDisplay is correctly pixel aligned
[Fixed] Shaders now mark as Changed when their samplers are modified
[Fixed] MouseState mouse wheel value now works in WinForms builds
[Fixed] Worked around an XNA bug with Instancing and Tiling on the xbox
[Fixed] WinForms host correctly creates a stencil buffer 
[Added] DrawTargets have a SurfaceDepthFormat property


----------------
-- 15/MAY/09
-- Version 1.6 (BETA)

[Improved] BETA: The shader custom tool has had a major overhaul, and now includes full source code
[Added] The 'XenShaderUpdate' tool has been added. This program recurses a directory and updates all out of date shaders it finds

[Fixed] A bug where nested particles were not always processed in the correct order
[Fixed] Many little bugs and minor improvements

NOTE: If updating from 1.5, make sure to run the prebuild!

----------------
-- 15/APR/09
-- Version 1.5.3

[Fixed] A bug where UpdateManager.Remove() would throw an exception when the object was already removed
[Fixed] A bug where GPU ParticleSystem rand and rand_smooth were swapped on the xbox, due to texture format differences
[Fixed] A bug where the Dpad was no longer being updated in PlayerInput

[Added] Element.TryGetLayout, which attempts to get the position and size of a 2D Element
[Added] TextElement and TextElementRect now share the ITextElement Interface
[Added] TextElement and TextElementRect now can have 2D Elements embedded between characters (AddInline)

----------------
-- 6/MAR/09
-- Version 1.5.2

[Fixed] Random distribution code for the GPU Particle Processor wasn't using an invariant culture when generating shader code, causing the compile to fail when a system number format such as German was used.

----------------
-- 3/MAR/09
-- Version 1.5.1

[Fixed] Fixed a bug in the model importer where some models would not import correctly if their content was entirely in the root node

----------------
-- 1/MAR/09
-- Version 1.5

Major update to many parts of the API

[Added] The Xen.Ex ParticleSystem 1.0 has been added, this is the largest addition to Xen.Ex yet
[Added] Particle System drawers for both 2D, 3D, billboard, lines and velocity based particle drawing
[Added] Particle System CPU and GPU processors and content builders
[Added] Xen Particle System content processor, including Particle System XML schema
[Added] Animation Override support (Manual Animation)
[Added] OcclusionQuery 'DrawPredicate' class
[Added] CullTestVisualizer class
[Added] Tutorials 18,19,21,22,23,24,25 added
[Added] BETA WinForms Tutorial mode (may be buggy if run many times)
[Added] Depth output shaders in Xen.Ex.Shaders (replaces the older, broken depth shaders)
[Added] ShaderOverride and LightCollection Draw Flags for forcing all models to use a specific shaders / lights,
[Added] BatchModel class for drawing non-animated models in large batches
[Removed] DeferredDraw() and Matrix Scaling detection have been compiled out of Xen, define XEN_EXTRA to get them back
[Improved] Animation Max Bone Count has been increased from 71 to 72 (due to Xbox compatibility improvements)
[Improved] Blur filters are now significantly more felixble (ranging from 3 sample to 31 sample filters)
[Improved] The DrawStatistics overlay has had a major facelift, it's now much simpler and colour coded
[Improved] Most Scene classes have had improvements or overhauls
[Improved] The shader compiler has seen significant upgrades and fixes
[Improved] The shader compiler works around bugs in Xbox 'AssembleFromSource()' (provided flow control isn't used)
[Improved] The animation system is now upto 30% faster on the Xbox and has better thread safety
[Fixed] Some preshader bugs with the shader compiler have been fixed
[Fixed] A number of minor bugs in the Animation System have been fixed
[Fixed] Two major bugs relating to low frequency updating in the UpdateManager have been fixed
[Fixed] Many, many minor fixes, performance improvements and tweaks throughout the API

-- 25/NOV/08
-- Version 1.4

[Fixed] A bug in the UpdateManager where first calls to Update() would have a bad DeltaTime
[Added] DrawState ProjectToScreen and ProjectFromScreen
[Added] Experimental WinForms support
[Added] DepthDrawSorter class, sorts drawn objects back to front / front to back
[Improved] Small enhancements to ICuller, added ICullableInstance


-- 9/NOV/08
-- Version 1.3a

[Improved] The keyboard input state 'TryGetKeyChar' method is now much more correct


-- 8/NOV/08
-- Version 1.3

[Added] KeyboardInputState class, treats keys as Button objects, for more flexibility
[Added] Tutorial 17 (Keyboard input)
[Added] BeginDrawBatch / EndDrawBatch in DrawState and the IBeginEndDrawBatch interface
[Fixed] World matrix inconsistencies when using DrawBatch and the CanDraw callback
[Fixed] Some bugs with text rendering
[Improved] Tutorial 09 now uses a shader to sample the texture


-- 3/NOV/08
-- Version 1.2a

[Fixed] The shader custom tool now works correctly with alternate system culture number formats
[Fixed] Centring the mouse works correctly with high frame rates when vsync is disabled


-- 31/OCT/08
-- Version 1.2

Updated to XNA 3.0
XNA 2.0 and XNA 3.0 BETA are no longer supported
All tools have been rebuilt for .NET 3.5

[Fixed] The model importer no longer falls over if the model doesn't have animations or materials


-- 25/OCT/08
-- Version 1.1

XBOX hardware instancing support added.
A number of bug fixes and tweaks, only one breaking change:

[BREAKING CHANGE] Changed IDrawBatch to correctly include the instance count

[Added] DrawFlags
[Added] StaticBinaryTreePartition and supporting classes
[Added] Tutorial 16
[Added] Projection cull pause
[Added] VFetch support on the xbox (see tutorial 16)
[Added] Hardware instancing emulation on the Xbox360
[Added] Hardware instancing support added to MaterialShader (xbox and PC)
[Fixed] DrawState.PushWorldTranslateMultiply now works correctly
[Fixed] Unregister FX actually works now
[Improved] MaterialShader is now more space efficient
[Improved] Shader Custom Tool updated
[Improved] Register FX is now part of the prebuild
[Improved] The prebuild is now much smarter, faster and has some bug fixes


04/OCT/08
Version 1.0

Initial Release



-----------------------------------------------------------------------------------------------------
-- Build notes:

Prebuild will build the xen DEBUG binaries.
Xen DEBUG builds perform a lot of extra runtime checks, and also provide extra statistics.

Xen RELEASE builds run faster, but provide fewer runtime checks.



-----------------------------------------------------------------------------------------------------
-- The CustomTool and unregister FX / prebuild:


Before trying to use xen, run the 'prebuild' batch file.


The 'unregister FX' batch file removes the Visual Studio shader plugin used by xen.
The 'prebuild' batch file registers the plugin and builds the debug xen libraries and model importer.







-----------------------------------------------------------------------------------------------------
-- Xna Notes:

Xen supports XNA Game Studio 3.1, Windows and the Xbox360.

Currently, support for XNA 4.0 is not planned due XNA 4 removing support for non-FX shaders.
Zune and Phone support is not planned.






-----------------------------------------------------------------------------------------------------
-- Xen Dlls:

There are 6 parts to xen:


Xen.dll 					-- the main API .dll
Xen.Ex.dll 					-- extensions, helpers and experimental classes
Xen.Graphics.ShaderSystem.dll 			-- shader interfaces


Xen.Ex.ModelImporter.dll 			-- Content pipeline model importer for Xen.Ex
Xen.Ex.ParticleSystem.dll 			-- Content pipeline particle system importer for Xen.Ex
Xen.Graphics.ShaderSystem.CustomTool.dll 	-- Visual Studio 2005/2008 CustomTool plugin



Most applications will use all three xen runtime .dlls and the content importers.
However if you are very brave the ShaderSystem could be used on its own.



-----------------------------------------------------------------------------------------------------
-- Visual Studio shader plugin:

Xen includes a Visual Studio custom tool for compiling shaders.
This is the largest part of the xen API.

A detailed description of how it works and how to use it can be found in Tutorial_03 (custom shader)


-----------------------------------------------------------------------------------------------------
-- Xen on a higher level...


Xen is an attempt to make using Xna a more reliable experience.
It is not a game engine, it is an API.

Because of this, xen replaces large portions of the Xna API.
This includes the XNA Game class, which makes xen less suitable for converting existing Xna projects.

Design decisions made in xen have larger scale project reliability in mind.

Some of the worst offenders in Xna are:

The Effect system,
Managing render targets,
Vertex/Index buffers (Declarations, streams, etc),
Render state management

Xen provides complete replacements for all of these, which form the major parts of the API.


-----------------------------------------------------------------------------------------------------
-- Getting started: The Tutorials project


The Tutorials project can be loaded by opening ./xen/Tutorials.sln, this uses the 'prebuild' xen build.

The Tutorials project is not flashy. It is a series of very small applications designed to show one
feature at a time, starting with the base Application class.


Take a moment to read them, they demonstrate the fundamentals in xen with a minimum of fuss.


Thank you!