import api from "../js/api.js";
import dateHelper from "../js/dateHelper.js";
import stateHandler from "../js/stateHandler.js";

export default class HintPlayer extends HTMLDivElement {
  static componentName = "hint-player";
  static observedAttributes = ["id"];

  constructor() {
    super();
  }

  connectedCallback() {
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (name === "id") {
      this.#createPlayer(newValue);
    }
  }

  #createPlayer(id) {
    this.classList.add(["hint-row"]);
    const rowChild = document.createElement("div");
    rowChild.setAttribute("id", `hint-${id}`);
    rowChild.classList.add(["hint"]);

    this.appendChild(rowChild);

    const audio = document.createElement("audio");
    audio.id = `target-${id}`;
    audio.classList.add(["player"]);
    audio.controls = true;

    const hintDisable = document.createElement("div");
    hintDisable.classList.add(["hint-disable"]);
    hintDisable.id = `hint-disable-${id}`;

    rowChild.appendChild(audio);
    rowChild.appendChild(hintDisable);

    const source = document.createElement("source");
    source.type = "audio/mp3";
    source.src = `${api.hint}/${dateHelper.createKeyForDate(stateHandler.getCurrentDate())}/1/${id}`;
    audio.appendChild(source);
  }
}
