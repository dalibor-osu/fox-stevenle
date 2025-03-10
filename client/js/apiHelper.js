
const postJSON = async (path, data) => {
  const result = await fetch(`${path}`, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-type": "application/json; charset=UTF-8"
    }
  });
  
  if (result.ok) {
    return await result.json();
  } else {
    console.error(result);
    return null;
  }
}

export { postJSON };
