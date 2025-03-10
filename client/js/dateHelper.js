const createKeyForDate = (date) => {
  let month = date.getUTCMonth() + 1;
  if (month < 10) {
    month = `0${month}`;
  }
  return `${date.getUTCFullYear()}-${month}-${date.getUTCDate()}`
}

const dateHelper = {
  createKeyForDate
}

export default dateHelper;
