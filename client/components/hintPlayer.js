import api from "../js/api.js";

export default class HintPlayer extends HTMLDivElement {
  static componentName = "hint-player";

  constructor() {
    super();
  }

  create(index, songNumber, dateKey) {
    this.classList.add(["hint-row"]);
    const rowChild = document.createElement("div");
    rowChild.setAttribute("id", `hint-${index}`);
    rowChild.classList.add(["hint"]);

    this.appendChild(rowChild);

    const audio = document.createElement("audio");
    audio.id = `target-${index}`;
    audio.classList.add(["player"]);
    audio.controls = true;

    const hintDisable = document.createElement("div");
    hintDisable.classList.add(["hint-disable"]);
    hintDisable.id = `hint-disable-${index}`;

    rowChild.appendChild(audio);
    rowChild.appendChild(hintDisable);

    const source = document.createElement("source");
    source.type = "audio/mp3";
    source.src = `${api.hint}/${dateKey}/${songNumber}/${index}`;
    audio.appendChild(source);
  }
}
