import stateHandler from "./stateHandler.js"
import guessResult from "./enums/guessResult.js";
import componentRegistry from "../components/componentRegistry.js";
import SongContainer from "../components/songContainer.js";
import api from "./api.js";
import { get } from "./apiHelper.js";
import dateHelper from "./dateHelper.js";

// localStorage.clear();

const hintCount = 3;

const canGuess = (state) => {
  const songState = state.songState[state.songIndex];
  return songState.guessIndex < 3 && !songState.guessHistory.includes(guessResult.success);
}

const renderState = (state) => {
  const progressText = document.getElementById("progress-text");
  const songState = state.songState[state.songIndex];

  if (songState.songInfo != null) {
    document.getElementById("song-container").render(songState.songInfo);
  }

  for (let i = 0; i < 3; i++) {
    const hintPlayer = document.getElementById(`hint-player-${i}`);
    hintPlayer?.create(i, state.songIndex + 1, dateHelper.createKeyForDate(stateHandler.getCurrentDate()));
  }

  document.getElementById("hint-disable-0")?.remove();

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
  window.transitionToPage = function(href) {
    document.querySelector('body').style.opacity = 0
    setTimeout(function() {
      window.location.href = href
    }, 200)
  }

  document.querySelector('body').style.opacity = 1

  const countdownInterval = setInterval(() => {
    const currentTime = new Date();
    const currentUTCDate = new Date(Date.UTC(currentTime.getUTCFullYear(), currentTime.getUTCMonth(), currentTime.getUTCDate(), currentTime.getUTCHours(), currentTime.getUTCMinutes(), currentTime.getUTCSeconds(), currentTime.getUTCMilliseconds()));
    const nextDayUTC = new Date(Date.UTC(currentTime.getUTCFullYear(), currentTime.getUTCMonth(), currentTime.getUTCDate() + 1));
    let difference = Math.ceil((nextDayUTC.getTime() - currentUTCDate.getTime()) / 1000);
    let hours, minutes, seconds = 0;

    if (difference / 3600 > 0) {
      hours = Math.floor(difference / 3600);
      difference -= 3600 * hours;
    }

    if (difference / 60 > 0) {
      minutes = Math.floor(difference / 60);
      difference -= 60 * minutes;
    }

    seconds = difference;

    const hoursText = hours > 9 ? hours : `0${hours}`;
    const minutesText = minutes > 9 ? minutes : `0${minutes}`;
    const secondsText = seconds > 9 ? seconds : `0${seconds}`;

    document.getElementById("timer-time").innerText = `${hoursText}:${minutesText}:${secondsText}`;
  }, 1000);

  const setupState = stateHandler.getInitializedState();
  const currentDate = stateHandler.getCurrentDate();
  const existsUrl = `${api.dailyQuizExistsByDate}/${dateHelper.createKeyForDate(currentDate)}`;
  const existsResult = await get(existsUrl);
  if (existsResult.status != 200 && window.location.search != null && window.location.search !== "") {
    window.location.search = "";
  }

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
