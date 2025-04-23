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
    console.log("Computer wählt:", computerAuswahl);

    if (spielerAuswahl === computerAuswahl) {
        console.log("Unentschieden");
    } else if (regeln[spielerAuswahl] === computerAuswahl) {
        console.log("Spieler gewinnt");
        spielerPunkte++;
    } else {
        console.log("Computer gewinnt");
        computerPunkte++;
    }

    console.log(`Punkte: Spieler ${spielerPunkte} : ${computerPunkte} Computer`);
}

// Testaufrufe
spielRunde("Feuer");
spielRunde("Wasser");
spielRunde("Pflanze");
