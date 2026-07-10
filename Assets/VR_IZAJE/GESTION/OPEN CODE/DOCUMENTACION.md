# VR INEFP — Documentación del Proyecto

Proyecto Unity con múltiples experiencias VR de formación.
Actualmente en transición del módulo de **Izaje** al módulo de **Fatiga y Somnolencia en Carreteras**.

**Motor:** Unity 2022.3+ · **Target:** Meta Quest / PC VR · **Pipeline:** URP
**Paquetes:** XR Interaction Toolkit 3.x, XR Core Utils, TextMeshPro, DOTween, Splines

---

## Estructura del proyecto

```
Assets/
├── VR_IZAJE/                  ← Módulo anterior (izaje). Se conserva por código compartido.
│   ├── CODIGO/                ← Scripts reutilizables
│   ├── ESCENAS/               ← Escenas del módulo de izaje
│   ├── 3D/                    ← Modelos, texturas, materiales de izaje
│   ├── PREFABS/
│   └── GESTION/OPEN CODE/     ← Documentación (este archivo)
├── VR_SOMNOLENCIA/            ← NUEVO MÓDULO: fatiga y somnolencia en carreteras
│   (pendiente de crear)
├── Settings/                  ← Configuración URP, calidad, iluminación
├── XR/                        ← Configuración OpenXR, loaders
├── XRI/                       ← Ajustes XR Interaction Toolkit
└── Samples/                   ← Samples de paquetes (XRI, Hands, etc.)
```

---

## Configuración del proyecto

| Parámetro | Valor |
|-----------|-------|
| Render Pipeline | URP (Performance y Quality assets) |
| Active Input Handling | Both (necesario para Input.GetKeyDown legacy) |
| HDR | Desactivado en Performance URP Config |
| MSAA | 4x |
| Luces adicionales | Per Pixel (activado) |
| Sombras suaves | Desactivadas en calidad baja (Quest) |
| Post-processing | Parcialmente activo (SSAO, Color Adjustments, Bloom en ajuste) |

### Advertencias conocidas
- "Active Input Handling is set to Both" en Android — ignorar, necesario por scripts legacy
- "Real-time indirect bounce shadowing only supported for directional light" — solucionado con Additional Lights en Per Pixel

---

## Scripts compartidos (reutilizables en VR_SOMNOLENCIA)

### Animacion

| Script | Ruta | Función |
|--------|------|---------|
| MirarCamara | `VR_IZAJE/CODIGO/Animacion/MirarCamara.cs` | Billboard: objeto mira siempre a cámara |
| NPCPatrol | `VR_IZAJE/CODIGO/Animacion/NPCPatrol.cs` | Patrullaje entre waypoints |
| OscilacionAutomatica | `VR_IZAJE/CODIGO/Animacion/OscilacionAutomatica.cs` | Oscilación por ejes (seno), con reset |
| OscilacionEmisivo | `VR_IZAJE/CODIGO/Animacion/OscilacionEmisivo.cs` | Oscila intensidad emisiva |
| OscilacionEscala | `VR_IZAJE/CODIGO/Animacion/OscilacionEscala.cs` | Oscila escala del objeto |
| OscilacionOpacidad | `VR_IZAJE/CODIGO/Animacion/OscilacionOpacidad.cs` | Oscila transparencia |
| RotacionAutomatica | `VR_IZAJE/CODIGO/Animacion/RotacionAutomatica.cs` | Rotación continua |

### Interacciones (sistema modular VR)

| Script | Ruta | Función |
|--------|------|---------|
| Button3D | `VR_IZAJE/CODIGO/Interacciones/Button3D.cs` | Botón 3D con hover/press/release y audio |
| CambioEscena | `VR_IZAJE/CODIGO/Interacciones/CambioEscena.cs` | Cambia de escena por nombre |
| CambioMaterialHover | `VR_IZAJE/CODIGO/Interacciones/CambioMaterialHover.cs` | Feedback hover: cambia material al apuntar |
| CuentaRegresiva | `VR_IZAJE/CODIGO/Interacciones/CuentaRegresiva.cs` | Desactiva objeto al terminar cuenta |
| DestructorPorTrigger | `VR_IZAJE/CODIGO/Interacciones/DestructorPorTrigger.cs` | Destruye objetos en trigger, reproduce sonido |
| RotacionArrastre | `VR_IZAJE/CODIGO/Interacciones/RotacionArrastre.cs` | Rotación trackball con láser VR |
| SimpleGrab | `VR_IZAJE/CODIGO/Interacciones/SimpleGrab.cs` | Agarre con retorno a posición base, eventos onGrab/onRelease |
| SistemaAutopartes | `VR_IZAJE/CODIGO/Interacciones/SistemaAutopartes.cs` | Selección de pieza con cambio de padre y toggle |
| SistemaDeMatch | `VR_IZAJE/CODIGO/Interacciones/SistemaDeMatch.cs` | Empareja objetos, activa pareja al coincidir |
| SwitchBoton | `VR_IZAJE/CODIGO/Interacciones/SwitchBoton.cs` | Toggle con listas interior/exterior |

### Sistema de pasos y diálogos

| Script | Ruta | Función |
|--------|------|---------|
| DialogData | `VR_IZAJE/CODIGO/Robot/DialogData.cs` | Datos: texto, audio, animaciones, listas de objetos |
| DialogPlayer | `VR_IZAJE/CODIGO/Robot/DialogPlayer.cs` | Reproductor con timeline, lip sync, auto-avance |
| LipSyncController | `VR_IZAJE/CODIGO/Robot/LipSyncController.cs` | Sincronización de labios con espectro de audio |
| CondAgarreTiempo | `VR_IZAJE/CODIGO/Sistema_pasos/CondAgarreTiempo.cs` | Avanza al mantener agarrado X tiempo, barra de progreso |
| CondAnimacion | `VR_IZAJE/CODIGO/Sistema_pasos/CondAnimacion.cs` | Espera animación, luego llama a TransicionVr |
| CondBoton | `VR_IZAJE/CODIGO/Sistema_pasos/CondBoton.cs` | Avanza al presionar botón |
| CondSeleccion | `VR_IZAJE/CODIGO/Sistema_pasos/CondSeleccion.cs` | Avanza al seleccionar N objetos con SimpleGrab |
| CondTiempo | `VR_IZAJE/CODIGO/Sistema_pasos/CondTiempo.cs` | Avanza tras un tiempo |
| CondTrigger | `VR_IZAJE/CODIGO/Sistema_pasos/CondTrigger.cs` | Avanza al entrar N objetos en trigger |

### VR

| Script | Ruta | Función |
|--------|------|---------|
| VRHandController | `VR_IZAJE/CODIGO/VR/VRHandController.cs` | Láser con mirilla, detecta SimpleGrab/Button3D/RotacionArrastre/etc. |
| DesktopInputController | `VR_IZAJE/CODIGO/VR/DesktopInputController.cs` | Modo escritorio (tecla P), rotación de cámara con flechas |

### Otros

| Script | Ruta | Función |
|--------|------|---------|
| FechaTexto | `VR_IZAJE/CODIGO/Otros/FechaTexto.cs` | Muestra fecha actual en TextMeshPro |
| SistemaAnimacionCorto | `VR_IZAJE/CODIGO/Otros/SistemaAnimacionCorto.cs` | Trigger: dispara animación "Accidente", activa/desactiva objetos |
| TransicionVr | `VR_IZAJE/CODIGO/Otros/TransicionVr.cs` | Fundido a negro DOTween, teletransporta objeto, auto-desactiva |

---

## Flujo de selección VR (VRHandController)

VRHandController.HandleGrab() detecta componentes en este orden:
1. **SimpleGrab** → agarre del objeto
2. **RotacionArrastre** → rotación por arrastre
3. **Button3D** → botón clickeable
4. **SistemaAutopartes** → toggle de modo al soltar gatillo

**CambioMaterialHover** se detecta de forma independiente.

---

## Flujo de diálogos

1. DialogPlayer.Start() activa `dialogList[0]`
2. DialogData.OnEnable() llama a DialogPlayer.Play()
3. Play() reproduce audio, texto, animaciones, timeline
4. Al terminar: procesa objetosToActivateOnEnd/DeactivateOnEnd
5. Si autoAvanzar=true, llama a Avanzar() → siguiente diálogo
6. CondTiempo / CondBoton / etc. llaman a Avanzar() externamente

---

## Archivos críticos de configuración

- `Assets/Settings/Project Configuration/Performance URP Config.asset` — URP de rendimiento (Quest)
- `Assets/Settings/Project Configuration/Quality URP Config.asset` — URP de calidad (PC)
- `Assets/Settings/Project Configuration/New Lighting Settings.lighting` — Config de iluminación
- `Assets/Settings/DefaultVolumeProfile.asset` — Perfil de post-processing global
- `Assets/link.xml` — Evita stripping de DOTween, XR, URP en build Android

---

## Atajos de teclado (modo pruebas)

- **P** durante Play → Alterna VR / Escritorio
- **Flechas** → Rotan cámara (modo escritorio)
- **Mouse + click** → Mano/láser (modo escritorio)
- **T** → Prueba TransicionVr.ParpadearConTransporte()
- **E** durante diálogo → Salta audio y ejecuta acciones finales

---

## Estado del proyecto (07 Julio 2026)

### Completado (módulo Izaje)
- [x] Interacción base VR (láser, agarre, rotación, botones)
- [x] Sistema de diálogos con robot guía + lip sync
- [x] Feedback visual hover con cambio de material
- [x] SistemaAutopartes: selección de piezas
- [x] RotacionArrastre: rotación trackball
- [x] TransicionVr: fundido a negro con teletransporte
- [x] SistemaAnimacionCorto: trigger de accidente
- [x] CondAgarreTiempo: agarre prolongado con barra
- [x] Build funcional en Meta Quest 3
- [x] Post-processing funcionando en Quest (parcial)
- [x] Repositorio GitHub: https://github.com/OrigenDevs/INEFP

### Pendiente (nuevo módulo: Fatiga y Somnolencia en Carreteras)
- (pendiente de definir contenido y estructura)

---

## Cómo empezar con el nuevo módulo

1. Crear carpeta `Assets/VR_SOMNOLENCIA/` con estructura similar a VR_IZAJE
2. Reutilizar scripts de `Assets/VR_IZAJE/CODIGO/` según sea necesario
3. Para modificar scripts compartidos, tener cuidado de no romper el módulo de izaje
4. Usar el URP Asset de rendimiento para builds Quest
5. Configurar nuevas escenas en `File > Build Settings`

### Notas técnicas importantes
- DOTween requiere `DOTween.Init()` antes de usar (ya incluido en TransicionVr.Awake())
- Active Input Handling en Both: necesario para Input.GetKeyDown legacy
- En URP de rendimiento, sombras suaves de alta calidad no son compatibles con Oculus
- Luces adicionales (point/spot) requieren Per Pixel en URP Asset

---
## Reporte de cambios que causaron regresión en build Quest (láser rosa + inputs no funcionan)

El build funcionaba correctamente hasta la incorporación de cambios recientes. A continuación se listan las diferencias detectadas entre el estado funcional y el actual:

### 1. PreloadedAssets eliminados (ProjectSettings/ProjectSettings.asset)
Se vació la lista `preloadedAssets`:
- **Antes:** contenía `XRGeneralSettingsPerBuildTarget.asset` (Android Providers) y `OpenXRPackageSettings.asset`
- **Ahora:** `preloadedAssets: []`
- **Impacto:** El sistema XR se inicializaba antes de cargar la primera escena; sin precarga, puede no activarse correctamente en runtime en Android.

### 2. OpenXRPackageSettings.asset modificado
Unity reescribió la configuración al abrir el proyecto, introduciendo cambios automáticos:
- Features renombradas de "Standalone" a "Android" (ej: `AndroidXRPerformanceMetrics Standalone → Android`)
- Features añadidas/eliminadas de las listas de perfil Android y Standalone (ej: se agregaron `-7175732919195521793`, `-3530502343714857586`; se quitaron `1144022495435832864`, `-7861760786241378733`)
- Extension strings de AR Anchor actualizadas: `XR_ANDROID_trackables → XR_ANDROID_trackables XR_ANDROID_device_anchor_persistence`
- **Impacto:** Pueden faltar features necesarias para inputs o render en Quest.

### 3. Composición de capas (CompositionLayers) — Alpha Processing deshabilitado
El URP Asset tiene CompositionLayers sin Alpha Processing. El Project Validation advierte:
- *"Enable Alpha Processing on Universal Render Pipeline Asset under Post Processing to enable composition layer support."*
- **Impacto:** Materiales transparentes (como el láser del XR Interaction Toolkit) se renderizan en rosa en build.

### 4. Depth Submission Mode en None
En OpenXR settings (Android), `Depth Submission Mode` está en `None` en lugar de `Depth 16` o `Depth 24`.
- **Impacto:** Puede causar problemas de renderizado en Quest.

### 5. Múltiples Interaction Profiles habilitados
Están habilitados simultáneamente:
- `Meta Quest Touch Plus Controller Profile`
- `Oculus Touch Controller Profile`
Unity advierte: *"Only enable interaction profiles that you actually test, to ensure their input bindings are complete."*
- **Impacto:** Posibles conflictos de bindings de input entre perfiles.

### 6. Screen Space Ambient Occlusion (SSAO) activado
El URP Asset tiene SSAO habilitado. Project Validation advierte:
- *"Using the Screen Space Ambient Occlusion render feature results in significant performance overhead when the application is running natively on device."*
- **Impacto:** Rendimiento degradado, posible causa indirecta de problemas de render.

### 7. Offscreen Rendering Only (Vulkan) activado
En OpenXR settings, `Offscreen Rendering Only (Vulkan)` está marcado.
- **Impacto:** Puede afectar cómo se renderizan los overlays y la interfaz.

### Correcciones recomendadas (por prioridad)
1. Habilitar **Alpha Processing** en el URP Asset (Post Processing)
2. Reducir a un solo **Interaction Profile** (el que coincida con el dispositivo target)
3. Cambiar **Depth Submission Mode** a `Depth 16`
4. Desactivar **SSAO** del URP Asset (o crear variante sin SSAO para Android)
5. Restaurar **preloadedAssets** en ProjectSettings
6. Verificar/restaurar el archivo **OpenXRPackageSettings.asset** desde el commit funcional (`cfff501`)
7. Revisar si **Offscreen Rendering Only (Vulkan)** es necesario
