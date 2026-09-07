import { Button } from "@mui/material";
import { useNavigate, useParams, useRouteLoaderData } from "react-router-dom";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

export default function NewDetails() {
  const params = useParams();
  const navigate = useNavigate();
  const privateDetails = useRouteLoaderData("private-new-details");
  const publicDetails = useRouteLoaderData("public-new-details");
  const news = privateDetails || publicDetails;

  const formatDate = (isoString) => {
    if (!isoString) return "N/A";
    const date = new Date(isoString);
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      second: "2-digit",
      hour12: true, // Change to false for 24-hour format
    });
  };

  async function handleDelete() {
    const response = await fetch(
      `https://localhost:7262/api/News/${params.id}`,
      {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      }
    );

    if (!response.ok) {
      console.log("delete failed");
    } else {
      navigate("/user/news");
    }
  }

  return (
    <div
      style={{
        maxWidth: "1330px",
        margin: "auto",
        padding: "35px",
        backgroundColor: "white",
      }}
    >
      {localStorage.getItem("email") === "admin@example.com" && (
        <div
          style={{
            display: "flex",
            gap: "10px",
            justifyContent: "flex-end",
          }}
        >
          <Button
            variant="contained"
            color="primary"
            onClick={() => {
              navigate("edit");
            }}
          >
            <EditIcon />
          </Button>
          <Button variant="contained" color="error" onClick={handleDelete}>
            <DeleteIcon />
          </Button>
        </div>
      )}
      <h1 style={{ margin: 0, color: 'green' }}>{news.title}</h1>
      <p style={{ marginTop: 0, color: "rgb(150, 150, 150)" }}>Posted {formatDate(news.createdDate)}</p>
      <div style={{ width: "100%", overflow: "hidden", margin: "20px 0" }}>
        <div style={{ textAlign: "center", maxWidth: "850px", margin: "auto" }}>
          <img
            src={news.photoUrl ? `https://localhost:7262/${news.photoUrl}` : "/Images/emptyPhoto.png"}
            alt="news-img"
            style={{ minWidth: "inherit", margin: "0px", width: "100%" }}
          />
        </div>
      </div>
      <div style={{ margin: "auto", maxWidth: "850px" }}>
        <div
          dangerouslySetInnerHTML={{ __html: news.content }}
          style={{ padding: "20px", backgroundColor: "rgb(245, 245, 245)", fontSize: "20px", borderLeft: "6px solid green", borderRadius: "6px" }}
        />
      </div>
    </div>
  );
}

export async function loader({ params }) {
  const response = await fetch(`https://localhost:7262/api/News/${params.id}`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${localStorage.getItem("token")}`,
      "Content-Type": "application/json",
    },
  });

  if (!response.ok) {
    throw new Response("Failed to fetch news", { status: response.status });
  }

  const resData = await response.json();
  return resData;
}
