### Continuation

Une fois les données au bon format dans mes classes respectives, il était temps de les enregistrer dans un fichier JSON de sortie.

#### Setup

J'ai donc commencé par renommer toutes mes variables grâces au package [Newtonsoft] avec l'attribut 
```
[JsonPropertyName("JsonAttributName")]
```
Afin de les traduire en anglais, car l'ACPR prend un JSON anglais, mais chez Henner, on code en français.

Une fois le renommage effectué, j'ai pu commencer à écrire dans un fichier JSON. En le vérifiant à la main (ou plutôt à l'oeil), je me suis dit que tout était correct. 

Après quelque recherches, je trouve le moyen de comparer un fichier JSON à un Schéma. Ce que je met en place évidemment. Et c'est le début des problèmes.

#### Contexte

Avant la réalisation de ce projet, la conversion Excel - JSON était effectué à la main. Donc vous pouvez vous en douter, des erreurs d'innatention, fautes de frappes peuvent arriver à la moindre occasions. Surtout quand le fichier JSON fait facilement près de 200 lignes.\
Le fichier JSON Schema contient également des petites fautes de frappes.

#### Retour Conversion

Une fois ces petites fautes de frappes réparées, bizarrement la comparaison fonctionne bien mieux, et me valide mon JSON de sortie.
Autant vous dire que j'étais content.

#### Informations Utilisateurs

J'en profite également pour informer l'utilisateur dans la console en cas de conversion/comparaison réussie, mais également lui montre les erreurs si besoin, s'il s'agit d'erreur de format, de données manquantes, ou d'erreur a la comparaison au schéma.
