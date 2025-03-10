import stateHandler from "./stateHandler.js"
import guessResult from "./enums/guessResult.js";
import componentRegistry from "../components/componentRegistry.js";

localStorage.clear();

const hintCount = 3;
let currentGuess = 1;

const setup = async () => {
  componentRegistry.register();
  const guessSubmitForm = document.getElementById("guess-form");
  guessSubmitForm.addEventListener("submit", async event => {
    event.preventDefault();
    const response = await stateHandler.handleGuess(event.target.firstElementChild.value);
    const progressText = document.getElementById("progress-text");
    if (response != null) {
      if (response.result === guessResult.success) {
        progressText.innerText += " 🟩";
      } else {
        progressText.innerText += " 🟥";
        if (currentGuess < hintCount) {
          document.getElementById(`hint-disable-${currentGuess}`).remove();
          currentGuess++;
        }
      }
    }
  })

  document.getElementById("hint-disable-0").remove();
}

await setup()
