import { Link } from "react-router-dom";
import { Typography, Chip } from "@mui/material";
import { CalendarToday, Update } from "@mui/icons-material";

export default function NewsCard({ title, date, isUpdated, image, content, to }) {
  const formatDate = (isoString) => {
    if (!isoString) return "";
    return new Date(isoString).toLocaleDateString("en-US", {
      month: "short",
      day: "numeric",
      year: "numeric"
    });
  };

  // Extract first paragraph for preview
  const previewText = content.replace(/<[^>]*>/g, "").split("\n")[0];
  const isLongText = previewText.length > 120;

  return (
    <Link to={to} className="news-card-link">
      <div className="news-card">
        {image && (
          <div 
            className="news-image"
            style={{ backgroundImage: `url(https://localhost:7262/${image})` }}
          ></div>
        )}
        <div className="news-content">
          <div className="news-header">
            <Typography variant="h5" className="news-title">
              {title}
            </Typography>
            <Chip
              icon={isUpdated ? <Update fontSize="small" /> : <CalendarToday fontSize="small" />}
              label={`${isUpdated ? "Updated" : "Published"} ${formatDate(date)}`}
              size="small"
              className="news-date-chip"
            />
          </div>
          <Typography variant="body1" className="news-preview">
            {isLongText ? `${previewText.substring(0, 120)}...` : previewText}
          </Typography>
          <div className="news-footer">
            <Typography variant="body2" className="read-more">
              Read full story →
            </Typography>
          </div>
        </div>
      </div>
    </Link>
  );
}