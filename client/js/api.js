const base = "/api";
const guess = `${base}/guess`;
const hint = `${base}/hint`;
const dailyQuiz = `${base}/dailyQuiz`;
const dailyQuizExistsByDate = `${dailyQuiz}/exists`;

const api = {
  base,
  guess,
  hint,
  dailyQuiz,
  dailyQuizExistsByDate
}

export default api;
