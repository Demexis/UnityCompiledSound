# UnityCompiledSound

## Overview
Plays a sound in the editor when compilation starts/ends.

This code is based on [JavadocMD/CompileIndicator.cs](https://gist.github.com/JavadocMD/39c2197c2b4970ed06a5247bee386c72), including fixes for the current LTS versions (2021/2022).

This repository was created in order to have a quick access to the package.

## Install
You can download a unity package from [the latest release](../../releases).

## Usage
All you need to do is create a folder called "*Resources*" anywhere under the "*Assets*" folder, and drop the audio files in there with the names "*StartCompilationSound*", "*EndCompilationSound*".

The package contains a sample of an already created folder with sounds.

*Note: After importing a package, the sound may not play immediately; it will appear after several recompilations.*
