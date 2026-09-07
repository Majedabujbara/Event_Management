export async function getEvents() {
  try {
    const response = await fetch("https://localhost:7262/api/Events");

    if (!response.ok) {
      console.error(`Error: ${response.statusText} (${response.status})`);
      return null;
    }

    const resData = await response.json();
    return resData;
  } catch (error) {
    console.error("Error fetching events:", error);
    return null;
  }
}

export async function getOrgs() {
  try {
    const response = await fetch("https://localhost:7262/api/Organizations");

    if (!response.ok) {
      console.error(`Error: ${response.statusText} (${response.status})`);
      return null;
    }

    const resData = await response.json();
    return resData;
  } catch (error) {
    console.error("Error fetching orgs:", error);
    return null;
  }
}

export async function getNews() {
  try {
    const response = await fetch("https://localhost:7262/api/News");

    if (!response.ok) {
      console.error(`Error: ${response.statusText} (${response.status})`);
      return null;
    }

    const resData = await response.json();
    return resData;
  } catch (error) {
    console.error("Error fetching news:", error);
    return null;
  }
}

export async function getBlogs() {
  try {
    const response = await fetch("https://localhost:7262/api/Blogs");

    if (!response.ok) {
      console.error(`Error: ${response.statusText} (${response.status})`);
      return null;
    }

    const resData = await response.json();
    return resData;
  } catch (error) {
    console.error("Error fetching orgs:", error);
    return null;
  }
}

export async function getComments(id, token) {
  try {
    const response = await fetch(`https://localhost:7262/api/Comments/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      console.error(`Error: ${response.statusText} (${response.status})`);
      return null;
    }

    const resData = await response.json();
    return resData;
  } catch (error) {
    console.error("Error fetching comments:", error);
    return null;
  }
}

export const searchAll = async (query) => {
  try {
    const res = await fetch(
      `https://localhost:7262/api/search?query=${encodeURIComponent(query)}`
    );

    if (!res.ok) {
      throw new Error(`HTTP error! status: ${res.status}`);
    }

    const data = await res.json();
    return {
      events: data.events || [],
      news: data.news || [],
      orgs: data.orgs || [],
    };
  } catch (err) {
    console.error("Search failed:", err);
    return { events: [], news: [], orgs: [] };
  }
};
