import { Button, IconButton } from "@mui/material";
import ThumbUpAltIcon from "@mui/icons-material/ThumbUpAlt";
import ChatIcon from "@mui/icons-material/Chat";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";

import DeleteIcon from "@mui/icons-material/Delete";
import { getComments } from "../utility/apiGetCalls";
import Input2 from "../components/Input2";
import SendIcon from "@mui/icons-material/Send";

function formatDate(dateString) {
  const date = new Date(dateString);
  const day = date.getDate().toString().padStart(2, "0");
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const year = date.getFullYear();
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");

  return `${day}/${month}/${year} ${hours}:${minutes}`;
}

export default function BlogDetails() {
  const navigate = useNavigate();
  const params = useParams();
  const [blog, setBlog] = useState(null);
  const [comments, setComments] = useState([]);
  const [commentEmpty, setCommentEmpty] = useState(false);
  const [isLiked, setIsLiked] = useState(false);

  async function handleDelete(commentId) {
    try {
      console.log(commentId);
      const response = await fetch(
        `https://localhost:7262/api/Comments/${commentId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );

      if (response.ok) {
        console.log("Comment deleted successfully.");
        getAllComments();
      } else {
        console.error("Failed to delete comment:", response.statusText);
      }
    } catch (error) {
      console.error("An error occurred while deleting the comment:", error);
    }
  }

  async function handleSubmit(event) {
    event.preventDefault();
    let hasError = false;

    const fd = new FormData(event.target);
    const data = Object.fromEntries(fd.entries());

    if (!data.content.trim()) {
      setCommentEmpty(true);
      hasError = true;
    } else {
      setCommentEmpty(false);
    }

    if (hasError) return;

    try {
      const res = await fetch(
        `https://localhost:7262/api/Comments/${params.id}`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
          body: JSON.stringify({
            content: data.content,
          }),
        }
      );

      if (!res.ok) {
        console.log(res.status);
      } else {
        event.target.reset();
        getAllComments();
      }
    } catch (error) {
      console.error(error);
    }
  }

  const getAllComments = async () => {
    const data = await getComments(params.id, localStorage.getItem("token"));
    if (data) {
      const sortedComments = data.sort(
        (a, b) => new Date(a.timeCommented) - new Date(b.timeCommented)
      );
      setComments(sortedComments);
    }
  };

  useEffect(() => {
    getAllComments();
  }, [params.id]);

  useEffect(() => {
    async function getBlogById() {
      try {
        const res = await fetch(
          `https://localhost:7262/api/Blogs/${params.id}`
        );
        if (res.ok) {
          const resData = await res.json();
          setBlog(resData);
        } else {
          console.log(res.status);
        }
      } catch (error) {
        console.error(error);
      }
    }

    getBlogById();
  }, [params.id]);

  const userId = localStorage.getItem("userId");

  useEffect(() => {
    if (blog && blog.likes) {
      const liked = blog.likes.some((like) => like.id === userId);
      setIsLiked(liked);
    }
  }, [blog, userId]);

  async function handleLike() {
    try {
      const res = await fetch(
        `https://localhost:7262/api/Blogs/${params.id}/like`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify(localStorage.getItem("userId")),
        }
      );

      if (res.ok) {
        setIsLiked(true);
        setBlog((prev) => ({
          ...prev,
          likes: [...prev.likes, { id: userId }],
        }));
      } else {
        console.error("Failed to like the blog:", res.statusText);
      }
    } catch (error) {
      console.error("Error while liking the blog:", error);
    }
  }

  async function handleUnLike() {
    try {
      const res = await fetch(
        `https://localhost:7262/api/Blogs/${params.id}/unlike`,
        {
          method: "POST",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
            "Content-Type": "application/json",
          },
          body: JSON.stringify(localStorage.getItem("userId")),
        }
      );

      if (res.ok) {
        setIsLiked(false);
        setBlog((prev) => ({
          ...prev,
          likes: prev.likes.filter((like) => like.id !== userId),
        }));
      } else {
        console.error("Failed to unlike the blog:", res.statusText);
      }
    } catch (error) {
      console.error("Error while unliking the blog:", error);
    }
  }

  if (!blog) return <p align="center">Loading...</p>;

  return (
    <>
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          gap: "35px",
          maxWidth: "800px",
          margin: "auto",
          padding: "35px 35px 115px 35px",
        }}
      >
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
              onClick={isLiked ? handleUnLike : handleLike}
              color="success"
              variant={isLiked ? "contained" : "outlined"}
            >
              {blog?.likes?.length || 0}&nbsp; <ThumbUpAltIcon />
            </Button>
            <Button
              color="success"
              disabled
              onClick={() => navigate(`/user/blogs/${blog.blogId}`)}
            >
              {comments.length}&nbsp; <ChatIcon />
            </Button>
          </div>
        </div>
        <div>
          <h5>Comments</h5>
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
            <div
              style={{ display: "flex", flexDirection: "column", gap: "10px" }}
            >
              {comments.length === 0 && (
                <span>Be the first one to add comment!</span>
              )}
              {comments.map((comment) => (
                <div key={comment.id}>
                  <div style={{ display: "flex", gap: "15px" }}>
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        background: "green",
                        width: "50px",
                        height: "50px",
                        borderRadius: "50%",
                        color: "white",
                        fontSize: "30px",
                      }}
                    >
                      {comment.commentatorName.charAt(0)}
                    </div>
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "column",
                        gap: "3px",
                        flex: 1,
                      }}
                    >
                      <div
                        style={{
                          backgroundColor: "rgb(228, 228, 228)",
                          padding: "6px",
                          borderRadius: "8px",
                        }}
                      >
                        <div
                          style={{
                            display: "flex",
                            justifyContent: "space-between",
                          }}
                        >
                          <div>
                            <div
                              style={{ fontWeight: "bolder", fontSize: "25px" }}
                            >
                              {comment.commentatorName}
                            </div>
                            <div style={{ fontSize: "20px" }}>
                              {comment.content}
                            </div>
                          </div>
                          {(comment.userId === localStorage.getItem("userId") ||
                            localStorage.getItem("email") ===
                              "admin@example.com") && (
                            <div>
                              <IconButton
                                aria-label="delete"
                                onClick={() => handleDelete(comment.id)}
                              >
                                <DeleteIcon />
                              </IconButton>
                            </div>
                          )}
                        </div>
                      </div>
                      <div
                        style={{
                          color: "rgb(156, 156, 156)",
                          fontSize: "13px",
                        }}
                      >
                        {formatDate(comment.timeCommented)}
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>

      <form onSubmit={handleSubmit}>
        <div className="comment">
          <Input2
            style={{ flex: 1 }}
            label="Add comment"
            name="content"
            error={commentEmpty}
          />
          <Button type="submit" variant="contained" color="success">
            <SendIcon />
          </Button>
        </div>
      </form>
    </>
  );
}
