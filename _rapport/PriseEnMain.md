---
layout: post
title:  "PriseEnMain"
date:   2025-07-21 22:11:13 +0200
categories: jekyll update
description: "Integrations"
permalink: /rapport/PriseEnMain
---

### Prise en main sujet

#### Outils Données
- Fichier Excel exemple
- Fichier Excel des règles à respecter
- Fichier JSON resultat (exemple)
- Fichier JSON Schema (pour verifier le format)

J'ai d'abord commencé par prendre en main les outils à ma disposition (Visual Studio et TortoiseGit). Bien que j'avais déja utilisé Visual Studio auparavent, l'extension Resharper m'a grandement aidé à découvrir de nouvelles utilités/manières de faire. Quand a TortoiseGit, ce logiciel permet de passer par l'explorateur de fichier plutôt que par le terminal afin d'envoyer les fichiers sur GitLab.

Tout darbord, comprendre le sujet. Il m'a fallu commencé par analyser le fichier Excel entré, le fichier JSON sorti, ainsi que les rêgles à respecter a la fois pour la conversion, mais aussi pour le fichier en sortie. Chaque élèment à son format, certains sont obligatoires ou non. Beaucoup de contraintes donc.

---
#### Fichiers Excel

Le fichier Excel est séparé en 3 sections (+1 qui ne contient qu'une seule valeur, indiquant combien de sections sont remplies, je l'appelerai Indice de Sections pour les explications).\
Chaque sections est dépendantes de celles avant.\
Exemple : \
-> Si l'Indice de Section indique 3, alors les sections 1, 2 et 3 doivent être renseignées.\
-> Mais, si l'Indice de Section indique 1, alors seulement la section 1 nous intéresse, peu importe si les sections 2 et 3 sont renseignées, on ne les considérera pas.

Chaque section est divisé en une 20-30aine de champs (1.1,1.2 etc...).\
Chaque champ peut donc contenir une ou plusieurs valeurs, indiqué par le fichier de règles fournis par Jordan. 

---

#### Fichiers JSON

Ce qui est bien avec les fichiers JSON, c'est qu'ils sont très explicites. Sur le format des objets en tous cas. En analysant le fichier Sortie, mais aussi le fichier Schéma, j'ai pu trouver les objets que je devrais mettre en place, mais ausssi le type attendu ainsi que le format. J'ai également trouvé des [RegEx](https://fr.wikipedia.org/wiki/Expression_r%C3%A9guli%C3%A8re), que j'utiliserais le moment venu.

---