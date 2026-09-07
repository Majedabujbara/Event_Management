import { Button } from "@mui/material";
import Input2 from "./Input2";
import { useNavigate, useParams } from "react-router-dom";
import { useState } from "react";
import TextA from "./TextA";

function formatHtml(html) {
  if (!html) return;

  const formatted = html
    .replace(/></g, ">\n<")
    .trim();

  return formatted;
}

export default function NewForm({ n, method }) {
  const navigate = useNavigate();
  const params = useParams();

  const [titleEmpty, setTitleEmpty] = useState(false);
  const [contentEmpty, setContentEmpty] = useState(false);
  const [photoEmpty, setPhotoEmpty] = useState(false);

  async function handleSubmit(e) {
    e.preventDefault();

    const fd = new FormData(e.target);
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

    const newsPhotoFile = fd.get("NewsPhoto");

    if (method === "POST" && (!newsPhotoFile || newsPhotoFile.size === 0)) {
      setPhotoEmpty(true);
      hasError = true;
    } else {
      setPhotoEmpty(false);
    }

    if (hasError) return;

    let url = "https://localhost:7262/api/News";

    if (method === "PATCH") {
      url = `https://localhost:7262/api/News/${params.id}`;
    }

    const response = await fetch(url, {
      method,
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: fd,
    });

    if (!response.ok) {
      console.log(await response.json());
    } else {
      navigate("/user/news");
    }
  }

  return (
    <div className="news-form-container">
      <form onSubmit={handleSubmit} className="news-form">
        <div className="form-header">
          <h2 className="form-title">
            {method === "POST" ? "Create News Article" : "Edit News Article"}
          </h2>
        </div>

        <div className="form-content">
          <Input2
            label="Article Title"
            type="text"
            name="Title"
            defaultValue={n?.title || ""}
            error={titleEmpty}
            errorText="Please fill this field"
            fullWidth
          />

          <TextA
            label="Content"
            name="Content"
            defaultValue={formatHtml(n?.content) || ""}
            error={contentEmpty}
            errorText="Please fill this field"
          />

          <div className="image-input-group">
            <Input2
              InputLabelProps={{ shrink: true }}
              label="Featured Image"
              type="file"
              name="NewsPhoto"
              fullWidth
              error={photoEmpty}
              errorText="Please pick an image"
              inputProps={{ accept: "image/*" }}
            />
          </div>

          <Input2
            style={{ display: "none" }}
            name="PhotoUrl"
            defaultValue={n?.photoUrl || ""}
          />

          <Button
            variant="contained"
            type="submit"
            className="submit-btn"
            color="success"
          >
            {method === "POST" ? "Publish News" : "Update Article"}
          </Button>
        </div>
      </form>
    </div>
  );
}
