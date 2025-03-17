import HintPlayer from "./hintPlayer.js";
import SongContainer from "./songContainer.js";

const components = [
  HintPlayer,
  SongContainer
]

const register = () => {
  for (const component of components) {
    if (component.prototype instanceof HTMLDivElement) {
      window.customElements.define(component.componentName, component, { extends: "div" });
      continue;
    }
    window.customElements.define(component.componentName, component);
  }
}

const componentRegistry = {
  register
}

export default componentRegistry;
