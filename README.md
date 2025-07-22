# ACPR-DORA

[TOC]

## Description

L'application console ACPR-DORA écrite en C# dotnet 8.0 permet de prendre les informations d'un fichier Excel et de les transférer en format json à destination de l'ACPR dans le cadre de la réglementation Dora.

---
**Note:**

>#### ACPR = Autorité de Contrôle Prudentiel et de Résolution
>C’est une autorité administrative française, adossée à la Banque de France, qui supervise les secteurs de la banque et de l’assurance. 
Elle veille à la stabilité financière, à la protection des clients et au respect des règles prudentielles par les établissements financiers.
>
> [Plus d'infos](https://acpr.banque-france.fr/fr)

> #### DORA = Digital Operational Resilience Act
>Il s’agit d’un règlement européen (UE 2022/2554) qui entre en application le 17 janvier 2025. Il vise à renforcer la résilience opérationnelle numérique des entités du secteur financier. Cela inclut :
>- La gestion des risques informatiques,
>- Le reporting des incidents majeurs,
>- Les tests de résilience,
>- La gestion des risques liés aux prestataires de services informatiques tiers.
>
> [Plus d'infos](https://www.eiopa.europa.eu/digital-operational-resilience-act-dora_en?prefLang=fr&etrans=fr)
---
## Utilisation

Compiler le programme avec 
```
dotnet build
```

Puis récupérez **ACPRConsole.exe** *(il peut se trouver dans ACPRConsole/bin/debug/net8.0/ACPRConsole.exe)*.

Placer les fichiers [**docs/regles.json**](docs/regles.json) et [**docs/schema.json**](docs/schema.json) au même endroit que **ACPRConsole.exe**

Ouvrez un terminal dans le répertoire de votre choix.

Puis tapez la commande suivante, en prenant soin de remplacer les arguments par les chemins des fichiers correspondants :

```
.\ACPRConsole.exe [Parametre1] [Parametre2]
```

### Elément en entrée

#### Paramètres transmis
 ##### Parametre1
- Chemin relatif à l'emplacement du fichier **ACPRConsole.exe**, ou chemin absolu
- Fichier Excel au format .xlsx
- Conforme au format attendu (exemple [**docs/ExcelTemplate.xlsx**](docs/ExcelTemplate.xlsx))

 ##### Parametre2
- Chemin relatif à l'emplacement du fichier **ACPRConsole.exe**, ou chemin absolu
- Chemin vers un fichier Json, ou vers un dossier. 
- En cas de dossier, le fichier en sortie se nommera **save.json**


### Elément en sortie
- Déroulement de la conversion, réussite ou erreurs
- Fichier **logs.log** contenant les logs du déroulement de la conversion
- Fichier Json conforme au [**docs/schema.json**](docs/schema.json)
En cas d'erreur lors de l'exécution de l'application, vous serez informé d'ou vient l'erreur.

Il se peut qu'une erreur d'exécution de l'application arrive. Si elle contient aucune information, c'est qu'il s'agit d'une erreur du fichier de log.

Veuillez simplement relancer l'application.

Exemple d'utilisation :

```
.\AcprConsole.exe fichier.xlsx resultat.json
.\AcprConsole.exe EmplacementExcel/fichier.xlsx EmplacementResultat/
```

Vous pouvez également trouver un exemple de résultat de fichier json à [**docs/JsonExemple.json**](docs/JsonExemple.json)

## Ajouter des règles

Le fichier **docs/regles.json** contient les valeurs valables pour les champs correspondants. 

Si vous les modifiez, pensez également à mettre à jour le fichier **docs/schema.json**

Pensez également à les rajouter dans les fichiers correspondants dans le dossier **ACPRTests**.

## Package Nuget

- [ ] [ClosedXml](https://www.nuget.org/packages/closedxml/)
- [ ] [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/9.0.6)
- [ ] [Microsoft.Extensions.Logging.Console](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Console/9.0.6)
- [ ] [Newtonsoft.Json.Schema](https://www.nuget.org/packages/Newtonsoft.Json.Schema/4.0.1)

Pour les tests : 
- [ ] [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/17.14.1)
- [ ] [xunit](https://www.nuget.org/packages/xunit/2.9.3)
- [ ] [xunit.runner.visualstudio](https://www.nuget.org/packages/xunit.runner.visualstudio/3.1.1)

## Présentations Projets

### ACPR

#### Domaine

Emplacement des classes de données du rapport, ainsi que

- KeyWords -> Mots-clés des différents dictionnaires de valeurs, ainsi que des valeurs de vérifications ou de logs.
- ReglesJson -> Classe contenant les valeurs des champs correspondants, ainsi que les noms de ces champs dans des valeurs constantes.

Les classes ont été créées à partir du [**schema**](../docs/schema.json), pour simplifier la conversion en json.

Chaque classe contient une méthode Créer, ainsi que d'autres méthodes chacune.

Chaque méthode permet de vérifier à minima une valeur de la classe, afin qu'elle soit conforme au [**schema**](../docs/schema.json).

#### Helper

- ExcelHelper -> Récupère les valeurs du fichier excel
- ExtensionsHelper -> Méthodes d'extensions
- FichierHelper -> Méthodes de vérification de fichier valide
- JsonHelper -> Lecture/Ecriture fichiers Json. Validation Json
- LoggerHelper -> Singleton contenant la logique des logs en sortie
- ValidationHelper -> Méthodes de vérification du format des données extraites

#### Services
Emplacement de **ModeleService.cs**, responsable de la répartition des données et de la création du rapport

#### Ressources

Emplacement des messages de logs.



### ACPRConsole
Projet permettant d'effectuer la conversion de fichier.

### ACPRTests
Chaque fichier permet de tester une classe en particulier à l'exception de

- **ConversionTest.cs** qui permet de tester une conversion excel -> json.
- **HelperTest.cs** qui permet de tester la majorité des Helpers.

Attention lors de l'execution des tests, les fichiers logs peuvent poser des erreurs. Veuillez relancer les tests si ça arrive.







