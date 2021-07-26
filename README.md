# Hand Proximate User Interface (HPUI) Demo

![Sample gif](Images/out.gif)

This is the accompanying demo application for the paper [HPUI: Hand Proximate User Interfaces for One-Handed Interactions on Head Mounted Displays](). It's a Oculus Quest/ Oculus Quest 2 application implemented in Unity. The image above shows a few screenshots from the application. 

## Setting up demo
- Make sure the Oculus Quest/Oculus Quest 2 has [developer mode enabled](https://developer.oculus.com/documentation/unity/unity-enable-device/) and hand tracking is enabled.
- Once the project is cloned, follow the instructions for [Oculus Quest build settings](https://developer.oculus.com/documentation/unity/unity-conf-settings/#build-settings) to make sure the project is correctly configured. Note that the project uses the Unity XR plugin framework.
- Ensure the SampleScene (Assets/Scenes/SampleScene.unity) is set as the scene to built.
- Use the [OVR Build APK and Run](https://developer.oculus.com/documentation/unity/unity-build-android-tools/#ovr-build-apk-run) or [OVR Quick Scene Preview](https://developer.oculus.com/documentation/unity/unity-build-android-tools/#ovr-quick-scene) to build and deploy the application.

## Performance considerations
The current implementation of the deformable user interface is computationally expensive, hence the framerate would drop when using an interface with the deformable UI with the application deployed on the headset. Alternatively, the headset can be connected to the development pc via Oculus Link and play the scene in unity to interact with the scene. As the application would be running on the development pc it would have lesser framerate drops. Note that Oculus Link is [compatible only with specific graphics cards](https://support.oculus.com/articles/headsets-and-accessories/oculus-link/oculus-link-compatibility).

## To site this work
```
```

### Disclosure
This repo uses
- [Furnished Cabin](https://assetstore.unity.com/packages/3d/environments/urban/furnished-cabin-71426) by Johnny Kasapi
- Oculus SDK: Copyright © Facebook Technologies, LLC and its affiliates. All rights reserved;
- Ray cursor: Copyright 2019 Marc Baloup, Géry Casiez, Thomas Pietrzak (Université de Lille, Inria, France)

## License
Project published under MIT license (see `LICENSE`)
