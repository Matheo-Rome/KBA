# Scene

Notre projet est composé de 2 scènes :
- La scène "Start" permettant de lancer une partie, voir les contrôles et le but du jeu
- La scène "Forest" comprenant l'arène de jeu et qui renvoie à la scène start en fin de partie.

Les scènes sont situées dans le dossier Assets/Scenes qui comprend les deux scènes mentionnées ci-dessus ainsi qu'une scène de test que nous avons utilisé lors du développement.

# Scripts

Dans le dossier Assets/Script, nous pouvons retrouver les différents scriptes utilisés dans notre projet, répartis par catégorie :
- Le script GlobalAchievement.cs qui gère l'obtention des achievements.
- Le dossier Canvas qui contient les différents scripts gérant les éléments d'UI du projet (bouton, scoreboard,...)
- Le dossier Combat qui contient les différents scripts gérants les combats. Le script principal est le Gun.cs qui s'occupe d'envoyer la bonne balle (dégât ou guérison),
d'instancier les différents effets de VFX et de changer la texture de la baguette magique en fonction des derniers sorts lancés.
- Le dossier Controllers qui contient les différents scripts gérants le comportement des entités du jeu. Il est notamment composé du script EnemyController qui va
permettre aux ennemies de s'approcher du joueur et de l'attaquer s'il est à porté. Le script Wander(Hostile) gérant les déplacements des entités quand aucun joueur n'est à porté.
- Le dossier Player qui contient les différents scripts gérants les déplacements et la caméra du joueur. Nous avons notamment le script PlayerMovement.cs qui s'occupe de gérer les
déplacements basiques du joueur et son évolution dans l'espace de jeu. Le script Swing.cs s'occupe de gérer la partie grappin du joueur.

# Quoi regarder

Dans la scène "Forest" les trois gameObject principaux à regarder pour commencer sont :
- Player : contient le script de déplacement et a pour fils, son collider, une caméra ayant elle-même pour fils les deux baguettes magiques.
- Mimics : Content tous les ennemis de la scène, chacun équipé des scriptes mentionné plus haut (à partir du rigidbody, les élements au-dessus viennent de l'asset de l'ennemie lui même)
- Canvas : contient tous les éléments d'UI : la barre de magie (Fuel), le crosshair, le scoreboard de fin et le gestionnaire d'Achievement. 
