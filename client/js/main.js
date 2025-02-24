import stateHandler from "./stateHandler.js"

const hintCount = 3;
let currentGuess = 1;

const setup = async () => {
  const guessSubmitForm = document.getElementById("guess-form");
  guessSubmitForm.addEventListener("submit", event => {
    event.preventDefault();
    const correct = stateHandler.handleGuess(event.target.firstElementChild.value);
    const progressText = document.getElementById("progress-text");
    if (correct) {
      progressText.innerText += " 🟩";
    } else {
      progressText.innerText += " 🟥";
      if (currentGuess < hintCount) {
        document.getElementById(`hint-disable-${currentGuess}`).remove();
        currentGuess++;
      }
    }
  })

  const hintsContainer = document.getElementById("hint-container");
  const hintContainer = hintsContainer.firstElementChild;

  // Create hint players from "template"
  for (let i = 1; i < hintCount; i++) {
    const currentContainer = hintContainer.cloneNode(true);
    currentContainer.id = `hint-${i}`;
    currentContainer.firstElementChild.id = `target-${i}`;
    currentContainer.firstElementChild.firstElementChild.src = `${i}.mp3`;
    const currentHintDisable = currentContainer.firstElementChild.children.namedItem("hint-disable-0");
    currentHintDisable.id = `hint-disable-${i}`;
    hintsContainer.appendChild(currentContainer);
  }

  document.getElementById("hint-disable-0").remove();

  // Transform players into VidstackPlayers
  for (let i = 0; i < hintCount; i++) {
  }
}

await setup()
