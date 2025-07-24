---
layout: post
title:  "Debut"
date:   2025-07-20 22:11:13 +0200
categories: jekyll update
description: "Integrations"
permalink: /rapport/Debut
---

### Début Projet

Une fois le sujet pris en main, et quelques questions posées à Jordan, j'ai pu enfin commencer à coder.

#### Package Nuget

- [ClosedXml](https://www.nuget.org/packages/closedxml/)
  > Ce package Nuget me permet de lire un fichier Excel, de récupérer les données, mais également (bien que inutile pour ce projet) d'écrire ou de modifier ces données.
- [Newtonsoft.Json.Schema](https://www.nuget.org/packages/Newtonsoft.Json.Schema/4.0.1)
  > Ce package me permet d'écrire en format JSON, mais aussi de personaliser les noms des attributs JSON de mes objets. Il me permet ainsi de développer en français, mais d'avoir un JSON en anglais à la fin.

#### Définitions

J'ai ainsi débuté par réaliser les differentes classe basées sur les objets JSON que j'avais repéré. Ces classes me permettront de pouvoir les transformer en JSON tout en gardant la structure des objets.\
Grâce au package [Newtonsoft](#package-nuget), j'ai pu renommer mes propriètés de classe en anglais uniquement lors de la conversion JSON.

#### Lecture fichier Excel

Grâce au package [ClosedXML](#package-nuget), j'ai pu commencer à lire le fichier Excel exemple qu'on m'a donné. Je me suis vite rendu compte d'une erreur possible. Je ne peux pas l'ouvrir dans mon programme si il est en cours d'utilisation par une autre application (Excel ici). Une fois le fichier fermé, j'ai pu commencer à extraire les données du fichier, et pour le moment simplement les afficher dans la console. \
Pour le moment, je ne me concentre uniquement sur la 1ère section du fichier. C'est selon moi la plus importante, car qu'importe l'Indice de Section, celle ci est obligatoire.

Une fois que l'affichage des données me plait, je décide de les envoyer dans mes classes respectives. Sans vérification de format pour le moment.\
Malheuresement ca ne fonctionnait pas, car j'avais déjà défini les types des propriétés.

J'ai donc rajouté un formattage de données pour mes classes, afin que les données soient au bon format et respectent les règles (obligatoire ou non). J'ai du vérifier pour chaque champs le format, le type ainsi que l'exigence de présence. 

Une fois fait, je pouvais donc ainsi m'attaquer à l'étape suivante : la conversion en JSON.