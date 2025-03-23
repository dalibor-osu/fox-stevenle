import stateHandler from "../js/stateHandler.js";
import htmlUtils from "../util/html.js";

export default class SongContainer extends HTMLElement {
  static componentName = "song-container";

  constructor() {
    super();
  }

  render(songInfo, renderButton) {
    const mainContainer = htmlUtils.createElementWithAttributes("div", { class: "song-container center" });

    const title = htmlUtils.createElementWithAttributes("p");
    title.innerText = songInfo.title;

    const authors = htmlUtils.createElementWithAttributes("p");
    authors.classList.add(["authors"]);
    authors.innerText = songInfo.authors;

    const link = htmlUtils.createElementWithAttributes("a", { href: songInfo.url, target: "_blank" });
    link.appendChild(title);

    if (songInfo.coverUrl != null && songInfo.coverUrl != "") {
      const image = htmlUtils.createElementWithAttributes("img", { src: songInfo.coverUrl, class: "song-image" })
      mainContainer.appendChild(image);
    }
    mainContainer.appendChild(link);
    mainContainer.appendChild(authors);

    if (renderButton) {
      var nextSongButton = htmlUtils.createElementWithAttributes("button");
      nextSongButton.innerText = "Next";
      nextSongButton.onclick = () => stateHandler.goToNextSong();
      mainContainer.appendChild(nextSongButton);
    }

    this.appendChild(mainContainer);
  }

  clear() {
    this.innerHTML = "";
  }
}
