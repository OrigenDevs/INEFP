# OPEN CODE — Guía de inicio (Módulo Somnolencia)

Punto de entrada para la sesión de Open Code (modelo Big Pickle).
Proyecto: **Fatiga y Somnolencia en Carreteras** — experiencia VR de formación.

## Documentación principal
→ Leer `GESTION/OPEN CODE/DOCUMENTACION.md` (código compartido, configuración, estado)

## Reglas importantes

1. **No modificar** archivos de `Assets/VR_IZAJE/` a menos que sea código compartido y no rompa el módulo anterior
2. **Contenido nuevo** va en `Assets/VR_SOMNOLENCIA/` (crear si no existe)
3. **Scripts reutilizables** están en `Assets/VR_IZAJE/CODIGO/` — revisarlos antes de crear nuevos
4. **Escenas nuevas** agregarlas en `File > Build Settings`
5. **Build Android:** Usar Performance URP Config, HDR desactivado

## Archivos críticos

### Configuración del proyecto
- `Assets/Settings/Project Configuration/Performance URP Config.asset`
- `Assets/Settings/Project Configuration/Quality URP Config.asset`
- `Assets/link.xml`
- `Assets/Settings/DefaultVolumeProfile.asset`

### Shared scripts más usados
- `VR_IZAJE/CODIGO/VR/VRHandController.cs`
- `VR_IZAJE/CODIGO/VR/DesktopInputController.cs`
- `VR_IZAJE/CODIGO/Interacciones/Button3D.cs`
- `VR_IZAJE/CODIGO/Interacciones/SimpleGrab.cs`
- `VR_IZAJE/CODIGO/Interacciones/CambioMaterialHover.cs`
- `VR_IZAJE/CODIGO/Robot/DialogPlayer.cs`
- `VR_IZAJE/CODIGO/Robot/DialogData.cs`
- `VR_IZAJE/CODIGO/Otros/TransicionVr.cs`

### Prefabs importantes
- `VR_IZAJE/PREFABS/05_INTERACTIVOS_SCRIPTS/VR_PLAYER.prefab` — XR Origin personalizado

---

## Comandos clave (modo pruebas)

- **P** durante Play → Alterna VR / Escritorio
- **Flechas** → Rotan cámara (modo escritorio)
- **Mouse + click** → Mano/láser (modo escritorio)
- **T** → Prueba TransicionVr

---

## Flujo de trabajo recomendado

1. Revisar DOCUMENTACION.md completo
2. Explorar scripts compartidos en VR_IZAJE/CODIGO/
3. Crear estructura VR_SOMNOLENCIA/
4. Reutilizar lo existente, crear lo nuevo

## Repositorio
https://github.com/OrigenDevs/INEFP
Rama: master
