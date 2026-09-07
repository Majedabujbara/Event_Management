import { useEffect, useState } from "react";
import NewsCard from "../components/NewsCard";
import { Button, Typography, Pagination, Box } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { Add } from "@mui/icons-material";
import { getNews } from "../utility/apiGetCalls";

export default function News() {
  const navigate = useNavigate();
  const [news, setNews] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const newsPerPage = 5;

  useEffect(() => {
    async function fetchAndSetNews() {
      const data = await getNews();
      if (data) {
        const sortedData = data.sort((a, b) => {
          const dateA = new Date(a.updatedDate || a.createdDate);
          const dateB = new Date(b.updatedDate || b.createdDate);
          return dateB - dateA;
        });
        setNews(sortedData);
        setCurrentPage(1);
      }
    }

    fetchAndSetNews();
  }, []);

  // Pagination logic
  const totalPages = Math.ceil(news.length / newsPerPage);
  const indexOfLastNews = currentPage * newsPerPage;
  const indexOfFirstNews = indexOfLastNews - newsPerPage;
  const currentNews = news.slice(indexOfFirstNews, indexOfLastNews);

  const handlePageChange = (event, value) => {
    setCurrentPage(value);
  };

  return (
    <div className="news-page">
      <div className="news-header">
        <Typography variant="h4" component="h1" className="page-title">
          University News
        </Typography>
        {localStorage.getItem("email") === "admin@example.com" && (
          <Button
            variant="contained"
            color="success"
            startIcon={<Add />}
            onClick={() => navigate("new")}
            className="add-news-btn"
          >
            Add News
          </Button>
        )}
      </div>

      {news.length === 0 ? (
        <Typography className="no-news-message">
          No news articles found
        </Typography>
      ) : (
        <>
          <div className="news-list">
            {currentNews.map((n) => (
              <NewsCard
                key={n.id}
                image={
                  n.photoUrl
                    ? n.photoUrl.replace(/\\/g, "/")
                    : "/Images/emptyPhoto.png"
                }
                title={n.title}
                date={n.updatedDate ? n.updatedDate : n.createdDate}
                isUpdated={!!n.updatedDate}
                content={n.content}
                to={n.id}
              />
            ))}
          </div>

          <Box display="flex" justifyContent="center" mt={4}>
            <Pagination
              count={totalPages}
              page={currentPage}
              onChange={handlePageChange}
              variant="outlined"
              sx={{
                "& .MuiPaginationItem-root": {
                  color: "green",
                  borderColor: "green",
                },
                "& .MuiPaginationItem-root.Mui-selected": {
                  backgroundColor: "green",
                  color: "white",
                  borderColor: "green",
                },
                "& .MuiPaginationItem-root.Mui-selected:hover": {
                  backgroundColor: "rgb(177, 255, 177)",
                  color: "black",
                },
                "& .MuiPaginationItem-root:hover": {
                  backgroundColor: "rgb(177, 255, 177)",
                  color: "black",
                },
              }}
            />
          </Box>
        </>
      )}
    </div>
  );
}
