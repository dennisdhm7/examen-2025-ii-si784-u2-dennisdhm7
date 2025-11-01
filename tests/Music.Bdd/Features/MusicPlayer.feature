# language: es
Característica: Control remoto de música
  Para controlar la reproducción
  Como usuario
  Quiero usar un control remoto con comandos

  Esquema del escenario: Ejecutar un comando desde el control
    Dado que tengo un reproductor de música
    Y tengo un control remoto
    Y configuro el comando "<comando>"
    Cuando presiono el botón del control
    Entonces debo ver el mensaje "<mensaje>"

    Ejemplos:
      | comando | mensaje                    |
      | play    | Playing the song.          |
      | pause   | Pausing the song.          |
      | skip    | Skipping to the next song. |

  Escenario: Error cuando no se configura un comando
    Dado que tengo un reproductor de música
    Y tengo un control remoto
    Cuando presiono el botón del control
    Entonces debe lanzarse un error por falta de comando
