import stateHandler from "./stateHandler.js"
import guessResult from "./enums/guessResult.js";
import componentRegistry from "../components/componentRegistry.js";
import SongContainer from "../components/songContainer.js";

// localStorage.clear();

const hintCount = 3;

const canGuess = (state) => {
  const songState = state.songState[state.songIndex];
  return songState.guessIndex < 3 && !songState.guessHistory.includes(guessResult.success);
}

const renderState = (state) => {
  const progressText = document.getElementById("progress-text");
  const songState = state.songState[state.songIndex];
  document.getElementById("hint-disable-0")?.remove();

  if (songState.songInfo != null) {
    document.getElementById("song-container").render(songState.songInfo);
  }

  songState.guessHistory.forEach((guess, i) => {
    if (guess == null) {
      return;
    }

    switch (guess) {
      case 0:
        progressText.innerText += " 🟩";
        break;
      case 1:
        progressText.innerText += " ⬛";
        document.getElementById(`hint-disable-${i + 1}`)?.remove();
        break;
      case 2:
        progressText.innerText += " 🟥";
        document.getElementById(`hint-disable-${i + 1}`)?.remove();
        break;
    }
  });
}

const setup = async () => {
  const setupState = stateHandler.getStateForDate();

  componentRegistry.register();
  const guessSubmitForm = document.getElementById("guess-form");

  renderState(setupState);

  const progressText = document.getElementById("progress-text");
  const songContainer = document.getElementById("song-container");

  guessSubmitForm.addEventListener("submit", async event => {
    event.preventDefault();
    if (!canGuess(stateHandler.getStateForDate())) {
      return;
    }

    const { response, state } = await stateHandler.handleGuess(event.target.firstElementChild.value);
    if (response != null) {
      if (response.result === guessResult.success) {
        progressText.innerText += " 🟩";
      } else {
        progressText.innerText += " 🟥";
        document.getElementById(`hint-disable-${state.songState[state.songIndex].guessIndex}`)?.remove();
      }
      if (response.song != null) {
        songContainer.render(response.song);
      }
    }
  })
}

await setup();
