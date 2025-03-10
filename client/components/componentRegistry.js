import HintPlayer from "./hintPlayer.js";

const components = [
  HintPlayer
]

const register = () => {
  for (const component of components) {
    window.customElements.define(component.componentName, component, { extends: "div" });
  }
}

const componentRegistry = {
  register
}

export default componentRegistry;
