import dateHelper from "./dateHelper.js";
import { postJSON } from "./apiHelper.js";
import api from "./api.js";
import guessResult from "./enums/guessResult.js";

const stateVersion = "beta";

const clear = () => {
  const sawDisclaimer = localStorage.getItem("disclaimer") != null;
  localStorage.clear();
  if (sawDisclaimer) {
    localStorage.setItem("disclaimer", "");
  }

  localStorage.setItem("version", stateVersion);
}

const isValidDate = (dateString) => {
  const regex = /^(?:19|20)\d{2}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12][0-9]|3[01])$/;
  return regex.test(dateString);
}
const getUrlBase = () => window.location.protocol + '//' + window.location.host + window.location.pathname;

const getCurrentDate = () => {
  const urlParams = new URLSearchParams(window.location.search);
  const date = urlParams.get("date");
  if (date == null) {
    return new Date(); // TODO: replace this with current date
  }

  if (!isValidDate(date)) {
    window.location.search = "";
    return;
  }

  return new Date(`${date}T00:00:00Z`);
}

const getCurrentSongNumber = () => {
  const urlParams = new URLSearchParams(window.location.search);
  const state = getStateForDate();

  if (!urlParams.has("song")) {
    return state.songIndex + 1;
  }

  let number = Number(urlParams.get("song"));
  if (isNaN(number) || number < 1 || number > 5) {
    urlParams.delete("song");
    window.transitionToPage(`${getUrlBase()}?${urlParams}`)
  }

  return number;
}

const getInitializedState = () => {
  const version = localStorage.getItem("version");
  if (version == null || version !== stateVersion) {
    clear()
  }

  const state = getStateForDate();
  let songNumber = getCurrentSongNumber();
  if (songNumber > 5) {
    songNumber = 5;
  }

  if (state.songIndex - 1 !== songNumber) {
    state.songIndex = songNumber - 1;
    setStateForDate(state);
  }


  return state;
}

const createDefaultStateForDate = (date) => {
  if (date == null) {
    console.error("Date can't be null");
    return null;
  }

  console.log("Creating default state for date", date);

  const defaultSongState = {
    success: false,
    guessIndex: 0,
    guessHistory: Array(3).fill(null),
    songInfo: null,
  }

  const state = {
    date: date,
    songIndex: 0,
    songState: Array(5).fill(null).map(() => structuredClone(defaultSongState)),
  }

  return structuredClone(state);
}

const goToSong = (songNumber) => {
  const urlParams = new URLSearchParams(window.location.search);
  songNumber = isNaN(songNumber) || songNumber < 1 || songNumber > 5 ? 1 : songNumber;
  urlParams.set("song", `${songNumber}`);
  window.transitionToPage(`${getUrlBase()}?${urlParams}`);
}

const goToNextSong = () => {
  const urlParams = new URLSearchParams(window.location.search);
  let songNumber = Number(urlParams.get("song"));
  songNumber = isNaN(songNumber) || songNumber === 0 ? 2 : songNumber + 1;
  goToSong(songNumber);
}

const getStateForDate = (date = null) => {
  date = date ?? getCurrentDate();
  const key = dateHelper.createKeyForDate(date);
  const state = window.localStorage.getItem(key);
  if (state == null) {
    const newState = createDefaultStateForDate(date);
    setStateForDate(newState, date);
    return newState;
  }
  return JSON.parse(state);
}

const setStateForDate = (state, date = null) => {
  date = date ?? getCurrentDate();
  const key = dateHelper.createKeyForDate(date);
  window.localStorage.setItem(key, JSON.stringify(state));
}

const handleGuess = async (guess, date = null) => {
  date = date ?? getCurrentDate();
  const state = getStateForDate(date);
  if (state == null) {
    console.error("Failed to get state for date", date);
    return null;
  }

  const songState = state.songState[state.songIndex];

  // Don't handle another guess if user already guessed 3 times
  if (songState.guessIndex > 2) {
    return null;
  }

  const dto = {
    text: guess,
    date: dateHelper.createKeyForDate(date),
    songIndex: state.songIndex,
    guessIndex: songState.guessIndex
  }

  const response = await postJSON(api.guess, dto);
  if (response.result === guessResult.success) {
    songState.success = true;
  }

  songState.guessHistory[songState.guessIndex] = response.result;
  songState.guessIndex++;
  if (response.song != null) {
    songState.songInfo = response.song;
  }

  setStateForDate(state, date);
  return { response, state };
}

const getResultText = (date) => {
  if (date == null) {
    date = getCurrentDate();
  }

  const state = getStateForDate(date);
  let text = `Fox Stevenle: ${dateHelper.createKeyForDate(date)}\n`;
  for (let i = 0; i < state.songState.length; i++) {
    const songState = state.songState[i];
    if (songState == null) {
      continue;
    }

    let songRow = "";
    for (const guess of songState.guessHistory) {
      switch (guess) {
        case guessResult.success:
          songRow += "🟩";
          break;
        case guessResult.skip:
          songRow += "⬛";
          break;
        case guessResult.fail:
          songRow += "🟥";
          break;
        default:
          continue;
      }
    }

    songRow += "\n";
    text += songRow;
  }

  text += "<https://foxstevenle.com>";
  navigator.clipboard.writeText(text);
}

const stateHandler = {
  getCurrentDate,
  getStateForDate,
  setStateForDate,
  handleGuess,
  goToNextSong,
  goToSong,
  getInitializedState,
  getCurrentSongNumber,
  clear,
  stateVersion,
  getResultText
};

export default stateHandler;
