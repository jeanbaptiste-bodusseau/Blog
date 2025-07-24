---
layout: post
title:  "Pratiques"
date:   2025-07-20 22:11:13 +0200
categories: jekyll update
description: "Integrations"
permalink: /rapport/Pratiques
---

### Bonnes pratiques

Je m'en vais montrer mon travail à Jordan, afin d'avoir un premier retour.\
Bien évidemment, il y a beaucoup de choses à revoir. Il me montre ainsi quelques pratiques qui sont bonnes à prendre.

#### Répartitions fichier

- Fichiers Helper\
  -> Fichier contenant de la logique pour une chose en particulier. Généralement, les helpers peuvent être réutilisé dans d'autres contexte d'applications. Il doivent être le plus générique que possible.\
  -> Après ces retours, j'ai donc 4 fichiers Helper\
  > JsonHelper.cs\
    -> Me permet de lire, écrire, et vérifier les fichiers JSON\
    > ExcelHelper.cs\
    -> Me permet de lire un fichier Excel et d'en extraire les données\
    > ValidationsHelper.cs\
    -> Me permet de vérifier les formats des données.\
    > ExtensionsHelper.cs\
    -> Méthodes d'extensions utiles, comme le formatage de string (trim, toLower etc...)

- Fichier Service\
  -> Fichier contenant la logique du programme. Ce fichier Service appelera donc les Helpers, et servira pour l'éxecution de l'application
  > ModeleService.cs\
  -> Service contenant la logique du programme. Grâce aux helpers, il récupère, formate, vérifie, et enregistre les données.
- Dossier Domaine\
  -> Dossier contenant les différentes classes. 
  >A noter qu'au lieu d'utiliser le constructeur de classe, il est préferable de coder une méthode Créer afin de pouvoir vérifier directement les valeurs rentrés. Ainsi on peut rendre le constructeur privé pour empécher d'instancier cette classe dans vérifier les valeurs auparavant.


```
    ┬ Projet
    ├───┬ Helpers
    │   ├──── JsonHelper.cs
    │   ├──── ExcelHelper.cs
    │   ├──── ExtensionsHelper.cs
    │   └──── ValidationHelper.cs
    ├───┬ Domaine
    │   ├──── Class1.cs
    │   ├──── Class2.cs
    │   └──── ...
    └───┬ Service
        └──── ModeleService.cs
```

#### Logger

Une fois l'arborescence de mon projet rectifié, j'ai réalisé une classe LoggerHelper (en Singleton), me permettant d'écrire les messages d'erreurs dans un fichier **.log**. Ce qui peut toujours être pratique.

Grâce à Jordan, j'ai pu également mettre en place un dossier de Ressources, me permettant d'y stocker des messages de réussites et d'échecs en différents langages en cas de perspective future. Bien que pour le moment, seulement les messages français sont mis en place.

Ainsi, lors de l'execution de l'application, l'utilisateur est au courant en cas de réussite de conversion, mais aussi en cas d'échecs, à la fois dans la console d'éxecution ainsi que dans unu fichier **.log**.