const createKeyForDate = (date) => {
  return `${date.getUTCFullYear()}-${date.getUTCMonth() + 1}-${date.getUTCDate()}`
}

const getStateForDate = (date = null) => {
  date = date ?? new Date();
  const key = createKeyForDate(date);
  const state = window.localStorage.getItem(key);
  if (state == null) {
    return null;
  }
  return JSON.parse(state);
}

const setStateForDate = (state, date = null) => {
  date = date ?? new Date();
  const key = createKeyForDate(date);
  window.localStorage.setItem(key, JSON.stringify(state));
}

const handleGuess = (guess) => {
  // TODO: Send request and evaluate
  return guess.toLowerCase() === "correct";
}

const stateHandler = {
  getStateForDate,
  setStateForDate,
  handleGuess
};
export default stateHandler;
