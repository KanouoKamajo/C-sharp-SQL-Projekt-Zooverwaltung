const pokemons = ["Feuer", "Wasser", "Pflanze"];
const regeln = {
  "Feuer": "Pflanze",
  "Wasser": "Feuer",
  "Pflanze": "Wasser"
};

let spielerPunkte = 0;
let computerPunkte = 0;

function spielRunde(spielerAuswahl) {
  const computerAuswahl = pokemons[Math.floor(Math.random() * pokemons.length)];

  let ergebnisText = `Spieler: ${spielerAuswahl} | Computer: ${computerAuswahl}<br>`;

  if (spielerAuswahl === computerAuswahl) {
    ergebnisText += "Unentschieden!";
  } else if (regeln[spielerAuswahl] === computerAuswahl) {
    ergebnisText += "Spieler gewinnt!";
    spielerPunkte++;
  } else {
    ergebnisText += "Computer gewinnt!";
    computerPunkte++;
  }

  ergebnisText += `<br>Punkte: Spieler ${spielerPunkte} : ${computerPunkte} Computer`;

  // Ausgabe im HTML anzeigen
  document.getElementById("ausgabe").innerHTML = ergebnisText;
}
