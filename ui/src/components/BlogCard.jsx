import { IconButton, Button } from "@mui/material";
import MoreHorizIcon from "@mui/icons-material/MoreHoriz";
import ThumbUpAltIcon from "@mui/icons-material/ThumbUpAlt";
import ChatIcon from "@mui/icons-material/Chat";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

function formatDate(dateString) {
  const date = new Date(dateString);
  const day = date.getDate().toString().padStart(2, "0");
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const year = date.getFullYear();
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");

  return `${day}/${month}/${year} ${hours}:${minutes}`;
}

export default function BlogCard({ blog, onOptionsClick, onLikeToggle }) {
  const navigate = useNavigate();
  const [isLiked, setIsLiked] = useState(false);

  const userId = localStorage.getItem("userId");

  useEffect(() => {
    const liked = blog.likes.some((like) => like.id === userId);
    setIsLiked(liked);
  }, [blog.likes]);

  function handleLikeClick() {
    if (localStorage.getItem("token")) {
      onLikeToggle(blog.blogId, isLiked);
    } else {
      navigate('/login')
    }
  }

  return (
    <div
      style={{
        padding: "1.5rem",
        background: "rgba(255, 255, 255, 0.05)",
        borderRadius: "1rem",
        backdropFilter: "blur(10px)",
        border: "1px solid rgba(255, 255, 255, 0.1)",
        fontFamily: "Arial, sans-serif",
        boxShadow: "0 4px 20px rgba(0,0,0,0.2)",
      }}
    >
      {/* Header */}
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          marginBottom: "1rem",
        }}
      >
        {/* User Info */}
        <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
          <div
            style={{
              backgroundColor: "green",
              color: "white",
              width: "60px",
              height: "60px",
              borderRadius: "50%",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              fontWeight: "bold",
              fontSize: "1.25rem",
              flexShrink: 0,
            }}
            aria-label={`Avatar of ${blog.userName}`}
          >
            {blog.userName.charAt(0).toUpperCase()}
          </div>
          <div>
            <div style={{ fontWeight: 600, fontSize: "1.5rem" }}>
              {blog.userName}
            </div>
            <div style={{ fontSize: "0.875rem", color: "#ccc" }}>
              {formatDate(blog.timePosted)}
            </div>
          </div>
        </div>

        {/* Options Icon */}
        {(blog.userId === localStorage.getItem("userId") ||
          localStorage.getItem("email") === "admin@example.com") && (
          <IconButton
            aria-label="Options"
            onClick={(e) => onOptionsClick(e, blog.blogId)}
            sx={{ alignSelf: "flex-start" }}
          >
            <MoreHorizIcon />
          </IconButton>
        )}
      </div>

      {/* Blog Content */}
      <div
        style={{
          fontSize: "1.125rem",
          lineHeight: 1.6,
          marginBottom: "1rem",
          whiteSpace: "pre-line",
        }}
      >
        <h3>{blog.title}</h3>
      </div>
      <div
        dangerouslySetInnerHTML={{ __html: blog.content }}
        style={{ marginBottom: "5px" }}
      />
      <img
        src={`https://localhost:7262/${blog.imagePath}`}
        style={{ marginBottom: "16px", width: "100%" }}
      />

      {/* Actions */}
      <div style={{ display: "flex", gap: "0.75rem" }}>
        <Button
          onClick={handleLikeClick}
          color="success"
          variant={isLiked ? "contained" : "outlined"}
        >
          {blog.likes?.length}&nbsp; <ThumbUpAltIcon />
        </Button>
        <Button
          color="success"
          onClick={() => navigate(`/user/blogs/${blog.blogId}`)}
        >
          {blog.comments?.length || 0}&nbsp; <ChatIcon />
        </Button>
      </div>
    </div>
  );
}
