import { useState, useEffect } from "react";
import { getBlogs } from "../utility/apiGetCalls";
import { Button, Fab, IconButton, Menu, MenuItem } from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import DeleteIcon from "@mui/icons-material/Delete";
import BlogCard from "../components/BlogCard";
import { Modal, Fade, Backdrop, Box } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import Input2 from "../components/Input2";
import CloseIcon from "@mui/icons-material/Close";
import TextA from "../components/TextA";

function formatHtml(html) {
  if (!html) return;

  const formatted = html.replace(/></g, ">\n<").trim();

  return formatted;
}

export default function Blogs() {
  const [anchorEl, setAnchorEl] = useState(null);

  const [blogs, setBlogs] = useState([]);
  const [selectedBlogId, setSelectedBlogId] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [editBlogData, setEditBlogData] = useState(null);

  const [titleEmpty, setTitleEmpty] = useState(false);
  const [contentEmpty, setContentEmpty] = useState(false);
  const [imageEmpty, setImageEmpty] = useState(false);

  const open = Boolean(anchorEl);

  const [modalOpen, setModalOpen] = useState(false);
  const handleOpen = () => {
    setTitleEmpty(false);
    setContentEmpty(false);
    setImageEmpty(false);
    setModalOpen(true);
  };
  const handleClose = () => {
    setModalOpen(false);
    setIsEditing(false);
    setEditBlogData(null);
  };

  async function fetchAndSetBlogs() {
    const data = await getBlogs();
    if (data) {
      const sortedData = data.sort((a, b) => {
        const dateA = new Date(a.timePosted);
        const dateB = new Date(b.timePosted);
        return dateB - dateA;
      });

      setBlogs(sortedData);
    }
  }

  useEffect(() => {
    fetchAndSetBlogs();
  }, []);

  const handleMenuClick = (event, blogId) => {
    setAnchorEl(event.currentTarget);
    setSelectedBlogId(blogId);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedBlogId(null);
  };

  const handleDelete = async () => {
    handleMenuClose();

    try {
      const res = await fetch(
        `https://localhost:7262/api/Blogs/${selectedBlogId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );

      if (res.ok) {
        setBlogs((prevBlogs) =>
          prevBlogs.filter((blog) => blog.blogId !== selectedBlogId)
        );
        console.log(`Blog with ID ${selectedBlogId} deleted successfully.`);
      } else {
        console.error("Failed to delete blog:", res.statusText);
      }
    } catch (error) {
      console.error("Error deleting blog:", error);
    }
  };

  async function handleSubmit(event) {
    event.preventDefault();

    const fd = new FormData(event.target);
    const data = Object.fromEntries(fd.entries());

    let hasError = false;

    if (!data.Title.trim()) {
      setTitleEmpty(true);
      hasError = true;
    } else {
      setTitleEmpty(false);
    }

    if (!data.Content.trim()) {
      setContentEmpty(true);
      hasError = true;
    } else {
      setContentEmpty(false);
    }

    if (!isEditing) {
      const blogPhotoFile = fd.get("Image");
      if (!blogPhotoFile || blogPhotoFile.size === 0) {
        setImageEmpty(true);
        hasError = true;
      } else {
        setImageEmpty(false);
      }
    }

    if (hasError) return;

    try {
      let res;

      if (isEditing) {
        // 🔥 Build a new FormData for PATCH manually
        const formData = new FormData();
        formData.append("Title", data.Title);
        formData.append("Content", data.Content);
        formData.append("ImagePath", editBlogData?.imagePath || "");

        const imageFile = fd.get("Image");
        if (imageFile && imageFile.size > 0) {
          formData.append("Image", imageFile);
        }

        res = await fetch(
          `https://localhost:7262/api/Blogs/${editBlogData.blogId}`,
          {
            method: "PATCH",
            headers: {
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
            body: formData,
          }
        );
      } else {
        // POST Request (creating new blog)
        res = await fetch("https://localhost:7262/api/Blogs", {
          method: "POST",
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
          body: fd,
        });
      }

      if (res.ok) {
        await fetchAndSetBlogs();
        setModalOpen(false);
        setIsEditing(false);
        setEditBlogData(null);
      } else {
        console.error("Failed to submit blog:", res.statusText);
      }
    } catch (error) {
      console.error(error);
    }
  }

  async function handleLikeToggle(blogId, isCurrentlyLiked) {
    console.log(blogId);
    const url = isCurrentlyLiked
      ? `https://localhost:7262/api/Blogs/${blogId}/unlike`
      : `https://localhost:7262/api/Blogs/${blogId}/like`;

    try {
      await fetch(url, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(localStorage.getItem("userId")),
      });

      await fetchAndSetBlogs();
    } catch (error) {
      console.error("Error toggling like:", error);
    }
  }

  return (
    <>
      {blogs.length === 0 && (
        <h4
          style={{
            backgroundColor: "rgb(237, 237, 237)",
            textAlign: "center",
            margin: "35px auto",
            width: "800px",
            padding: "35px",
            borderRadius: "15px",
          }}
        >
          There's no blogs right now check back later
        </h4>
      )}

      <div
        style={{
          display: "flex",
          flexDirection: "column",
          gap: "25px",
          maxWidth: "800px",
          margin: "auto",
          padding: "35px",
        }}
      >
        {blogs.map((blog) => (
          <BlogCard
            key={blog.blogId}
            blog={blog}
            onOptionsClick={handleMenuClick}
            onLikeToggle={handleLikeToggle}
          />
        ))}
      </div>

      <Menu
        anchorEl={anchorEl}
        open={open}
        onClose={handleMenuClose}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        transformOrigin={{ vertical: "top", horizontal: "right" }}
      >
        <MenuItem
          onClick={() => {
            const blogToEdit = blogs.find((b) => b.blogId === selectedBlogId);
            setEditBlogData(blogToEdit);
            setIsEditing(true);

            setTitleEmpty(false);
            setContentEmpty(false);
            setImageEmpty(false);
            setModalOpen(true);

            handleMenuClose();
          }}
        >
          <EditIcon sx={{ mr: 1, color: "green" }} />
          <span style={{ color: "green" }}>Edit Post</span>
        </MenuItem>
        <MenuItem onClick={handleDelete}>
          <DeleteIcon sx={{ mr: 1, color: "red" }} />
          <span style={{ color: "red" }}>Delete Post</span>
        </MenuItem>
      </Menu>

      {localStorage.getItem("token") && (
        <Fab
          color="success"
          aria-label="add"
          style={{
            position: "fixed",
            bottom: "2rem",
            right: "2rem",
            zIndex: 1000,
          }}
          onClick={handleOpen}
        >
          <AddIcon />
        </Fab>
      )}

      <Modal
        open={modalOpen}
        onClose={(event, reason) => {
          if (reason !== "backdropClick" && reason !== "escapeKeyDown") {
            handleClose();
          }
        }}
        closeAfterTransition
        slots={{ backdrop: Backdrop }}
        slotProps={{
          backdrop: {
            timeout: 500,
          },
        }}
      >
        <Fade in={modalOpen}>
          <Box
            sx={{
              position: "absolute",
              top: "50%",
              left: "50%",
              transform: "translate(-50%, -50%)",
              width: {
                xs: "90%", // for small screens (0px - 600px)
                sm: 600, // for 600px and up
              },
              maxHeight: "80vh",
              overflowY: "auto",
              bgcolor: "background.paper",
              boxShadow: 24,
              p: 4,
            }}
          >
            <div style={{ position: "fixed", right: "10px", top: "10px" }}>
              <IconButton aria-label="close" onClick={handleClose}>
                <CloseIcon />
              </IconButton>
            </div>
            <h1>Add Blog</h1>
            <form onSubmit={handleSubmit}>
              <div
                style={{
                  display: "flex",
                  flexDirection: "column",
                  gap: "15px",
                  marginTop: "15px",
                  marginBottom: "15px",
                }}
              >
                <Input2
                  label="Title"
                  name="Title"
                  error={titleEmpty}
                  defaultValue={editBlogData?.title || ""}
                  errorText="Please fill this field"
                />
                <TextA
                  label="Content"
                  name="Content"
                  error={contentEmpty}
                  defaultValue={formatHtml(editBlogData?.content) || ""}
                  errorText="Please fill this field"
                />
                <Input2
                  label="Blog Photo"
                  name="Image"
                  InputLabelProps={{ shrink: true }}
                  type="file"
                  inputProps={{ accept: "image/*" }}
                  error={imageEmpty}
                  errorText="Please pick an image"
                />
                <Input2 style={{ display: "none" }} name="ImagePath" />
                <Button
                  type="submit"
                  style={{ marginTop: "15px" }}
                  color="success"
                  variant="contained"
                >
                  {isEditing ? "Edit Blog" : "Add Blog"}
                </Button>
              </div>
            </form>
          </Box>
        </Fade>
      </Modal>
    </>
  );
}
