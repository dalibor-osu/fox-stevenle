import dateHelper from "./dateHelper.js";
import { postJSON } from "./apiHelper.js";
import api from "./api.js";
import guessResult from "./enums/guessResult.js";

const testDate = new Date(2025, 1, 26);
const isValidDate = (dateString) => {
  const regex = /^(?:19|20)\d{2}-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12][0-9]|3[01])$/;
  return regex.test(dateString);
}

const getCurrentDate = () => {
  const urlParams = new URLSearchParams(window.location.search);
  const date = urlParams.get("date");
  if (date == null) {
    return testDate; // TODO: replace this with current date
  }

  if (!isValidDate(date)) {
    window.location.search = "";
    return;
  }

  return new Date(`${date}T00:00:00Z`);
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

const getStateForDate = (date = null) => {
  date = date ?? new Date();
  const key = dateHelper.createKeyForDate(date);
  const state = window.localStorage.getItem(key);
  if (state == null) {
    return createDefaultStateForDate(date);
  }
  return JSON.parse(state);
}

const setStateForDate = (state, date = null) => {
  date = date ?? new Date();
  const key = dateHelper.createKeyForDate(date);
  window.localStorage.setItem(key, JSON.stringify(state));
}

const handleGuess = async (guess, date = null) => {
  date = date ?? new Date();
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

  songState.guessHistory[songState.guessIndex++] = response.result;
  if (response.song != null) {
    songState.songInfo = response.song;
  }

  setStateForDate(state, date);
  return response;
}

const stateHandler = {
  getCurrentDate,
  getStateForDate,
  setStateForDate,
  handleGuess
};

export default stateHandler;
