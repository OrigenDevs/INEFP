# OPEN CODE — Guía de inicio

Punto de entrada para la instancia de Open Code (modelo Big Pickle).
Todo el proyecto vive dentro de `Assets/VR_IZAJE/` — no uses rutas fuera de aquí.

## Documentación principal
→ Leer `GESTION/OPEN CODE/DOCUMENTACION.md` (estructura, scripts, estado)

## Archivos críticos

### Scripts del sistema (Interacciones)
- `Assets/VR_IZAJE/CODIGO/Interacciones/Button3D.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/CambioEscena.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/CambioMaterialHover.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/CuentaRegresiva.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/DestructorPorTrigger.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/RotacionArrastre.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/SimpleGrab.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/SistemaAutopartes.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/SistemaDeMatch.cs`
- `Assets/VR_IZAJE/CODIGO/Interacciones/SwitchBoton.cs`

### Diálogos y robot
- `Assets/VR_IZAJE/CODIGO/Robot/DialogData.cs`
- `Assets/VR_IZAJE/CODIGO/Robot/DialogPlayer.cs`
- `Assets/VR_IZAJE/CODIGO/Robot/LipSyncController.cs`

### Otros scripts
- `Assets/VR_IZAJE/CODIGO/Otros/FechaTexto.cs`
- `Assets/VR_IZAJE/CODIGO/Otros/SistemaAnimacionCorto.cs`
- `Assets/VR_IZAJE/CODIGO/Otros/TransicionVr.cs`

### Animación
- `Assets/VR_IZAJE/CODIGO/Animacion/MirarCamara.cs`
- `Assets/VR_IZAJE/CODIGO/Animacion/NPCPatrol.cs`
- `Assets/VR_IZAJE/CODIGO/Animacion/OscilacionAutomatica.cs`
- `Assets/VR_IZAJE/CODIGO/Animacion/OscilacionEmisivo.cs`
- `Assets/VR_IZAJE/CODIGO/Animacion/OscilacionEscala.cs`
- `Assets/VR_IZAJE/CODIGO/Animacion/OscilacionOpacidad.cs`
- `Assets/VR_IZAJE/CODIGO/Animacion/RotacionAutomatica.cs`

### Sistema de pasos
- `Assets/VR_IZAJE/CODIGO/Sistema_pasos/CondAgarreTiempo.cs`
- `Assets/VR_IZAJE/CODIGO/Sistema_pasos/CondAnimacion.cs`
- `Assets/VR_IZAJE/CODIGO/Sistema_pasos/CondBoton.cs`
- `Assets/VR_IZAJE/CODIGO/Sistema_pasos/CondSeleccion.cs`
- `Assets/VR_IZAJE/CODIGO/Sistema_pasos/CondTiempo.cs`
- `Assets/VR_IZAJE/CODIGO/Sistema_pasos/CondTrigger.cs`

### VR
- `Assets/VR_IZAJE/CODIGO/VR/VRHandController.cs`
- `Assets/VR_IZAJE/CODIGO/VR/DesktopInputController.cs`

### Escenas
- `Assets/VR_IZAJE/ESCENAS/VEHICULOS DE CONSTRUCCION.unity`

---

## Flujo de trabajo (David + Juan)

### Roles
- **David:** Dueño del proyecto, toma decisiones, define contenido, revisa avances
- **Juan:** Desarrollador Unity, implementa código, monta escenas, prueba

### Ciclo diario
1. **Juan** abre el proyecto, revisa la documentación
2. **Juan** trabaja y hace commits con mensajes claros
3. **David** revisa avances y da feedback

### Commits
- Hacer commits pequeños por tarea
- Mensaje: `[Área] Descripción breve`

---

## Prioridades de trabajo

1. Leer scripts del sistema (Interacciones/)
2. Abrir la escena de VEHICULOS DE CONSTRUCCION.unity
3. Modelar y configurar componentes del camión
4. Configurar SistemaAutopartes en cada pieza
5. Configurar diálogos en DialogPlayer
6. Probar flujo completo en VR/escritorio
7. Build APK/PC

## Comandos clave
- **P** durante Play → Alterna VR / Escritorio
- **Flechas** → Rotan cámara (modo escritorio)
- **Mouse + click** → Mano/láser (modo escritorio)
