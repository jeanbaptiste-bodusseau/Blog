---
layout: page
title: A propos
permalink: /about/
---

### Mon Parcours

<div style="display: flex; align-items: flex-start; gap: 20px; flex-wrap: wrap; justify-content: center;">

<div style="text-align: center; max-width: 200px; ">
    <img src="{{ '/assets/images/henner_logo.jpg' | relative_url }}" style="max-height:95px;max-width:95px; border: 1px solid #ccc; border-radius: 10px;">
    <div style="margin-top: 8px;">
      <strong>Développeur informatique</strong><br>
      En alternance<br>
      (Septembre 2025 - ?)<br>
      Henner GMC - Equipe DSI<br>
      <a href="https://www.henner.com" target="_blank">Henner nantes</a>
    </div>
  </div>

<div style="display: flex; align-items: center; justify-content: center; height: auto; min-height: 100px;">
    <div style="font-size: 24px;">&#8592;</div>
  </div>

<div style="text-align: center; max-width: 200px;">
    <img src="{{ '/assets/images/ynov_logo.png' | relative_url }}" style="max-height:95px;max-width:95px; border: 1px solid #ccc; border-radius: 10px;">
    <div style="margin-top: 8px;">
      <strong>Bachelor Informatique<br>(en cours)</strong><br>
      Troisième année<br>
      <a href="https://www.ynov.com/campus/nantes" target="_blank">Ynov Campus - Nantes</a>
    </div>
  </div>

<div style="display: flex; align-items: center; justify-content: center; height: auto; min-height: 100px;">
    <div style="font-size: 24px;">&#8592;</div>
  </div>

<div style="text-align: center; max-width: 200px;">
    <img src="{{ '/assets/images/staubin_logo.png' | relative_url }}" style=" height:95px; border: 1px solid #ccc; border-radius: 10px;">
    <div style="margin-top: 8px;">
      <strong>Baccalauréat Général 2023</strong><br>
      Mathématique - NSI<br>
      Options Maths Experts<br>
      <a href="https://www.staubinlasalle.fr" target="_blank">Saint Aubin-La-Salle - Verrières-En-Anjou</a>
    </div>
  </div>

</div>

#### Autres Expériences

<div style="display: flex; align-items: flex-start; gap: 50px; flex-wrap: wrap; justify-content: center;">

<div style="text-align: center; max-width: 200px;">
    <img src="{{ '/assets/images/henner_logo.jpg' | relative_url }}" style="height:75px; border: 1px solid #ccc; border-radius: 10px;">
    <div style="margin-top: 8px;">
      <strong>Développeur Stagiare</strong><br>
      Henner GMC - Equipe DSI<br>
      <a href="https://www.henner.com" target="_blank">Henner nantes</a>
    </div>
  </div>

<div style="text-align: center; max-width: 200px;">
    <img src="{{ '/assets/images/montsorelli_logo.jpg' | relative_url }}" style="height:75px; border: 1px solid #ccc; border-radius: 10px;">
    <div style="margin-top: 8px;">
      <strong>Serveur</strong><br>
      Eté 2023 & été 2024<br>
      Restaurant Le Montsorelli - Montsoreau<br>
      <a href="https://www.hotel-lebussy.fr" target="_blank">Restaurant Le Montsorelli - Hotel Le Bussy</a>
    </div>
  </div>

<div style="text-align: center; max-width: 200px;">
    <img src="{{ '/assets/images/mediatheque_logo.png' | relative_url }}" style="height:75px; border: 1px solid #ccc; border-radius: 10px;">
    <div style="margin-top: 8px;">
      <strong>Bibliothécaire stagiaire </strong><br>
      Janvier 2020<br>
      Espace COOLturel - Divatte-sur-Loire<br>
      <a href="https://www.espacecoolturel.fr" target="_blank">Espace COOLturel</a>
    </div>
  </div>

</div>

### Mes Projets Réalisés

<div id="projets-carousel" class="projets-carousel" >

  <button id="prev-projet" class="projet-bouton projet-bouton-left">&#10094;</button>

  <div class="projets" style="overflow: hidden;">
    <div class="projets-track">
      <div class="projet-container">
        <div class="projet-video">
          <video src="{{ '/assets/videos/telesce.mp4' | relative_url }}" preload="auto" controls></video>
        </div>
        <div class="projet-legende">
          <h3><ins>Telesce</ins></h3>
          <p>Ce jeu de plateforme 2D à été réalisé en classe, avec l'aide de mon camarade <a href="https://blog-v0ja.onrender.com/blog" target="_blank">Aurélien Dugast</a>.</p>
          <h4>Technologies Utilisés</h4>
          <p>Unity - C#</p>
        </div>
      </div>
      <div class="projet-container">
        <div class="projet-video">
          <video src="{{ '/assets/videos/labyRogue.mp4' | relative_url }}" preload="auto" controls></video>
        </div>
        <div class="projet-legende">
          <h3><ins>Python Rogue</ins></h3>
          <p>Echapper vous de ce labyrinthe, en affrontant des ennemies à l'aide de cartes</p>
          <h4>Technologies Utilisés</h4>
          <p>Python</p>
        </div>
      </div>
      <div class="projet-container">
        <div class="projet-video">
          <video src="{{ '/assets/videos/rpg.mp4' | relative_url }}" preload="auto" controls></video>
        </div>
        <div class="projet-legende">
          <h3><ins>RPG "en ligne"</ins></h3>
          <p>Parcourez un labyrinthe infesté de monstres et de mini-jeu dans un système de combat tour par tour en équipe.</p>
          <h4>Technologies Utilisés</h4>
          <p>C# (avec une api pour gérer les monstres/jeu/récompenses)</p>
        </div>
      </div>
    </div>
  </div>

  <button id="next-projet" class="projet-bouton projet-bouton-right">&#10095;</button>
</div>
<script>
  const track = document.querySelector('.projets-track');
  const nextBtn = document.getElementById('next-projet');
  const prevBtn = document.getElementById('prev-projet');
  const total = document.querySelectorAll('.projet-container').length;
  let index = 0;
  function updatePosition() {
    track.style.transform = `translateX(-${index * 100}%)`;
  }
  function resetAllVideos() {
    const videos = document.querySelectorAll('video');
    videos.forEach(video => {
      video.pause();
      video.currentTime = 0;
    });
  }
  nextBtn.onclick = () => {
    index = (index + 1) % total;
    resetAllVideos();
    updatePosition();
  };
  prevBtn.onclick = () => {
    index = (index - 1 + total) % total;
    resetAllVideos();
    updatePosition();
  };
</script>
