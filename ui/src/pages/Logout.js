import { redirect } from "react-router-dom";

export async function action() {
  try {
    const response = await fetch("https://localhost:7262/api/Account/Logout");

    if (!response.ok) {
      console.error("Logout API failed");
    }
  } catch (error) {
    console.error("Logout request failed:", error);
  }

  localStorage.clear();
  return redirect("/");
}
