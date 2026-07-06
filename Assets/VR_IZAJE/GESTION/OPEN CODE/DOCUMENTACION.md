# VR IZAJE — Documentación del Proyecto

Aplicación VR para visualización interactiva de componentes de vehículos de construcción.
Experiencia guiada por robot asistente con selección, rotación y exploración de piezas.

**Motor:** Unity 2022.3+ · **Target:** Oculus Quest / PC VR · **Pipeline:** Built-in
**Paquetes:** XR Interaction Toolkit 3.x, XR Core Utils, TextMeshPro, DOTween

---

## Estructura del proyecto

```
Assets/VR_IZAJE/
├── CODIGO/
│   ├── Animacion/          MirarCamara, NPCPatrol, OscilacionAutomatica,
│   │                         OscilacionEmisivo, OscilacionEscala,
│   │                         OscilacionOpacidad, RotacionAutomatica
│   ├── Interacciones/      Button3D, CambioEscena, CambioMaterialHover,
│   │                         CuentaRegresiva, DestructorPorTrigger,
│   │                         RotacionArrastre, SimpleGrab, SistemaAutopartes,
│   │                         SistemaDeMatch, SwitchBoton
│   ├── Otros/              FechaTexto, SistemaAnimacionCorto, TransicionVr
│   ├── Robot/              DialogData, DialogPlayer, LipSyncController
│   ├── Shaders/            AutodeskInteractiveMasked_Custom_BACKUP.shadergraph
│   ├── Sistema_pasos/      CondAgarreTiempo, CondAnimacion, CondBoton,
│   │                         CondSeleccion, CondTiempo, CondTrigger
│   └── VR/                 DesktopInputController, VRHandController
├── ESCENAS/
│   ├── HOME.unity
│   ├── LOBBI.unity
│   ├── TRABAJO DE CAMPO IZAJE.unity
│   └── VEHICULOS DE CONSTRUCCION.unity
├── 3D/                     Modelos, animaciones, materiales, HDRI
├── PREFABS/                Prefabs estructurales, medianos, decorativos, interactivos
├── SONIDO/                 Archivos de audio
└── GESTION/                Documentación, reportes, notas
    ├── DAVID IZAJE/        Notas de contenido, guías, progreso
    ├── JUAN IZAJE/         Espacio de trabajo de Juan
    └── OPEN CODE/          Documentación, índice, reportes
```

---

## Scripts del sistema

### Animacion

| Script | Ruta | Función |
|--------|------|---------|
| MirarCamara | `CODIGO/Animacion/MirarCamara.cs` | Hace que el objeto mire siempre hacia la cámara principal (billboard) |
| NPCPatrol | `CODIGO/Animacion/NPCPatrol.cs` | Patrullaje de NPC entre waypoints |
| OscilacionAutomatica | `CODIGO/Animacion/OscilacionAutomatica.cs` | Oscilación suave por ejes independientes (seno), con reset público |
| OscilacionEmisivo | `CODIGO/Animacion/OscilacionEmisivo.cs` | Oscila intensidad emisiva con color |
| OscilacionEscala | `CODIGO/Animacion/OscilacionEscala.cs` | Oscila la escala del objeto |
| OscilacionOpacidad | `CODIGO/Animacion/OscilacionOpacidad.cs` | Oscila transparencia de material |
| RotacionAutomatica | `CODIGO/Animacion/RotacionAutomatica.cs` | Rotación continua automática |

### Interacciones (sistema modular)

| Script | Ruta | Función |
|--------|------|---------|
| Button3D | `CODIGO/Interacciones/Button3D.cs` | Botón 3D con hover/press/release y audio externo |
| CambioEscena | `CODIGO/Interacciones/CambioEscena.cs` | Cambia de escena por nombre |
| CambioMaterialHover | `CODIGO/Interacciones/CambioMaterialHover.cs` | Feedback visual: cambia material al hacer hover con el láser |
| CuentaRegresiva | `CODIGO/Interacciones/CuentaRegresiva.cs` | Desactiva el objeto al terminar cuenta regresiva |
| DestructorPorTrigger | `CODIGO/Interacciones/DestructorPorTrigger.cs` | Destruye objetos al entrar en trigger, reproduce sonido, activa objetos |
| RotacionArrastre | `CODIGO/Interacciones/RotacionArrastre.cs` | Rotación por arrastre con láser VR (trackball), sensibilidad configurable |
| SimpleGrab | `CODIGO/Interacciones/SimpleGrab.cs` | Agarre simplificado con retorno a posición base. Expone eventos onGrab/onRelease |
| SistemaAutopartes | `CODIGO/Interacciones/SistemaAutopartes.cs` | Selección de pieza con cambio de padre, transición suave y toggle de objetos |
| SistemaDeMatch | `CODIGO/Interacciones/SistemaDeMatch.cs` | Al entrar un objeto, desactiva/destruye ese objeto y activa su pareja. Al completar todos los pares, avanza el diálogo |
| SwitchBoton | `CODIGO/Interacciones/SwitchBoton.cs` | Toggle con listas de objetos modo interior/exterior |

### Sistema de pasos y diálogos

| Script | Ruta | Función |
|--------|------|---------|
| DialogData | `CODIGO/Robot/DialogData.cs` | Datos del diálogo: texto, audio, animaciones, listas de objetos |
| DialogPlayer | `CODIGO/Robot/DialogPlayer.cs` | Reproductor de diálogos con timeline, lip sync, auto-avance |
| LipSyncController | `CODIGO/Robot/LipSyncController.cs` | Sincronización de labios con espectro de audio, populate via AnimationClip |
| CondAgarreTiempo | `CODIGO/Sistema_pasos/CondAgarreTiempo.cs` | Avanza al mantener agarrado un objeto X tiempo, con barra de progreso y sonido loop |
| CondAnimacion | `CODIGO/Sistema_pasos/CondAnimacion.cs` | Asigna controller al animator, espera que termine la animación y llama a TransicionVr |
| CondBoton | `CODIGO/Sistema_pasos/CondBoton.cs` | Avanza al presionar un botón |
| CondSeleccion | `CODIGO/Sistema_pasos/CondSeleccion.cs` | Avanza al seleccionar N objetos distintos con SimpleGrab |
| CondTiempo | `CODIGO/Sistema_pasos/CondTiempo.cs` | Avanza al siguiente diálogo tras un tiempo |
| CondTrigger | `CODIGO/Sistema_pasos/CondTrigger.cs` | Avanza al entrar N objetos mínimos en un trigger |

### VR

| Script | Ruta | Función |
|--------|------|---------|
| VRHandController | `CODIGO/VR/VRHandController.cs` | Láser con mirilla, detecta SimpleGrab, Button3D, RotacionArrastre, CambioMaterialHover, SistemaAutopartes |
| DesktopInputController | `CODIGO/VR/DesktopInputController.cs` | Modo escritorio (tecla P) |

### Otros

| Script | Ruta | Función |
|--------|------|---------|
| FechaTexto | `CODIGO/Otros/FechaTexto.cs` | Muestra la fecha actual en un TextMeshPro |
| SistemaAnimacionCorto | `CODIGO/Otros/SistemaAnimacionCorto.cs` | Al entrar un trigger: dispara trigger "Accidente" en Animator del personaje, activa/desactiva listas de objetos |
| TransicionVr | `CODIGO/Otros/TransicionVr.cs` | Fundido a negro con DOTween, teletransporta objeto a destino, reproduce audio, se auto-desactiva al terminar |

---

## Flujo de selección VR (prioridad de detección)

VRHandController.HandleGrab() detecta componentes en este orden:
1. **SimpleGrab** → agarre del objeto
2. **RotacionArrastre** → rotación por arrastre
3. **Button3D** → botón clickeable (hover/press/release)
4. **SistemaAutopartes** → toggle de modo al soltar gatillo

**CambioMaterialHover** se detecta de forma independiente (funciona junto con cualquier componente).

---

## Flujo de diálogos

1. DialogPlayer.Start() activa `dialogList[0]`
2. Al activarse, DialogData.OnEnable() llama a DialogPlayer.Play()
3. Play() reproduce audio, texto sincronizado, animaciones, timeline
4. Al terminar: procesa objetosToActivateOnEnd/DeactivateOnEnd
5. Si autoAvanzar=true, llama a Avanzar() → desactiva actual, activa siguiente
6. CondTiempo / CondBoton / etc. llaman a Avanzar() para avanzar externamente

### LipSyncController
- Se asigna al robot mediante `DialogPlayer.lipSync` y `DialogPlayer.mouthRenderer`
- PopulateSpritesFromClip() extrae sprites de un AnimationClip o de la subcarpeta `BOCA LOOP`
- En Update: analiza espectro de audio, cicla sprites de boca si hay volumen > threshold

### CondAgarreTiempo
- Se suscribe a SimpleGrab.onGrab/onRelease
- Mientras se mantiene agarrado, acumula tiempo y actualiza escala de una barra
- Al completar, llama a DialogPlayer.Avanzar()
- Reproduce AudioSource en loop durante el agarre

### CondAnimacion
- En OnEnable: asigna RuntimeAnimatorController al Animator objetivo
- Espera que termine el clip actual
- Llama a TransicionVr.ParpadearConTransporte()

---

## Flujo de TransicionVr

1. El GameObject se activa (OnEnable)
2. `ParpadearConTransporte()` inicia secuencia DOTween:
   - Fundido a negro
   - Teletransporta `objetoATransportar` a `destino` (posición + rotación)
   - Reproduce `audioTransicion`
   - Espera
   - Fundido desde negro
3. Al completar: `gameObject.SetActive(false)` — queda listo para reutilizar

---

## Flujo de SistemaAnimacionCorto

1. Objeto con trigger collider + script
2. Cualquier collider entra al trigger → OnTriggerEnter:
   - `animatorPersonaje.SetTrigger("Accidente")`
   - Desactiva todos los objetos en `objetosADesactivar`
   - Activa todos los objetos en `objetosAActivar`

---

## Estado del proyecto (01 Julio 2026)

### Completado
- [x] Interacción base VR (láser, agarre, rotación, botones)
- [x] Sistema de diálogos con robot guía + lip sync
- [x] Feedback visual hover con cambio de material
- [x] SistemaAutopartes: selección de piezas con transición y toggle
- [x] RotacionArrastre: rotación tipo trackball con láser
- [x] CambioMaterialHover: cambio de material en hijos recursivo
- [x] DestructorPorTrigger + CuentaRegresiva + SistemaDeMatch
- [x] TransicionVr: fundido a negro con teletransporte y sonido
- [x] SistemaAnimacionCorto: trigger con animación de accidente y activación/desactivación de objetos
- [x] CondAgarreTiempo: agarre prolongado con barra de progreso
- [x] CondAnimacion: reproducción de animación antes de transición
- [x] Documentación actualizada con todos los scripts

### Pendiente — Vehículos de Construcción
- [ ] Modelado 3D del camión y componentes (Fase 1)
- [ ] Configurar SistemaAutopartes en cada pieza
- [ ] Grabar audios del robot para cada componente
- [ ] Escribir textos de diálogo por pieza
- [ ] Pruebas de flujo completo en VR
- [ ] Build para PC VR / Quest

---

## Para comenzar

1. Abrir proyecto en Unity 2022.3+
2. Ir a `ESCENAS/VEHICULOS DE CONSTRUCCION.unity`
3. Presionar P durante Play para modo escritorio
4. Leer scripts en `CODIGO/Interacciones/` y `CODIGO/Robot/`
5. Agregar CambioMaterialHover a objetos para feedback visual hover
6. Configurar diálogos en DialogPlayer.dialogList

### Atajos de teclado (modo pruebas)
- **P** durante Play → Alterna VR / Escritorio
- **Flechas** → Rotan cámara (modo escritorio)
- **Mouse + click** → Mano/láser (modo escritorio)
- **T** → Prueba TransicionVr.ParpadearConTransporte()
- **E** durante un diálogo → Salta el audio actual y ejecuta las acciones de finalización

### Notas técnicas
- SistemaAutopartes requiere: parteSeleccionada, padreInicial, padreObservacion
- RotacionArrastre requiere: SphereCollider en el objeto
- CambioMaterialHover busca MeshRenderers en Awake (jerarquía estática)
- VRHandController usa `laserMaxDistance` para el alcance del láser
- TransicionVr usa DOTween; requiere tener `objetoATransportar`, `destino` y `audioTransicion` asignados
- CondAgarreTiempo requiere SimpleGrab en el mismo GameObject
- CondAnimacion requiere que TransicionVr esté presente en la escena
