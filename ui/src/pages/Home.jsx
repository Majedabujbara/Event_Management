import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import Card from "../components/Card";
import { Button } from "@mui/material";
import OrgCard from "../components/OrgCard";
import Carousel from "../components/Carousel";
import { getEvents, getNews, getOrgs } from "../utility/apiGetCalls";

export default function Home() {
  const [events, setEvents] = useState([]);
  const [org, setOrg] = useState([]);
  const [news, setNews] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchAndSetEvents() {
      const data = await getEvents();
      if (data) {
        setEvents(data);
      }
    }
    fetchAndSetEvents();
  }, []);

  useEffect(() => {
    async function fetchAndSetOrgs() {
      const data = await getOrgs();
      if (data) {
        setOrg(data);
      }
    }
    fetchAndSetOrgs();
  }, []);

  useEffect(() => {
    async function fetchAndSetNews() {
      const data = await getNews();
      if (data) {
        const sortedNews = data
          .sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate))
          .slice(0, 5);
        setNews(sortedNews);
      }
    }
    fetchAndSetNews();
  }, []);

  return (
    <div className="home-container">
      {/* --- Beautiful Welcome Section Start --- */}
      <div
        style={{
          textAlign: "center",
          marginTop: "40px",
          background: "linear-gradient(135deg, #e0f2f1 0%, #ffffff 100%)",
          padding: "50px 20px",
          borderRadius: "20px",
          boxShadow: "0 8px 16px rgba(0, 0, 0, 0.1)",
        }}
      >
        <img
          src="/Images/BAULogo.png"
          style={{ marginBottom: "20px", width: "200px" }}
        />
        <h1
          style={{
            fontSize: "40px",
            color: "#2e7d32",
            marginBottom: "20px",
            fontWeight: "700",
            letterSpacing: "1px",
          }}
        >
          Welcome to the University Community!
        </h1>
        <p
          style={{
            fontSize: "20px",
            color: "#555",
            maxWidth: "800px",
            margin: "0 auto",
            lineHeight: "1.6",
          }}
        >
          Stay connected and inspired through our vibrant university community.
          Discover the latest events, connect with student organizations, stay
          updated with campus news, and dive into inspiring blog posts. Your
          journey of learning, leadership, and creativity begins here!
        </p>
        <Button
          variant="contained"
          color="success"
          size="large"
          sx={{
            marginTop: "30px",
            padding: "10px 30px",
            fontSize: "18px",
            borderRadius: "30px",
          }}
          onClick={() =>
            window.scrollTo({
              top: document.body.scrollHeight,
              behavior: "smooth",
            })
          }
        >
          Explore Events
        </Button>
      </div>
      {/* --- Beautiful Welcome Section End --- */}
      <Carousel news={news} />
      <div>
        <h2>Latest Events</h2>
        <div style={{ display: "flex", justifyContent: "flex-end" }}>
          <Button
            variant="outlined"
            color="success"
            onClick={() =>
              navigate(
                localStorage.getItem("token") ? "/user/events" : "/events"
              )
            }
            sx={{ fontSize: "16px", marginTop: "10px" }}
          >
            view more
          </Button>
        </div>
        <div className="event-wrapper">
          {(() => {
            const now = new Date();

            const ongoingEvents = events.filter(
              (evt) =>
                new Date(evt.startTime) <= now && new Date(evt.endTime) >= now
            );

            const upcomingEvents = events.filter(
              (evt) => new Date(evt.startTime) > now
            );

            const combined = [...ongoingEvents, ...upcomingEvents].slice(0, 4); // Max 4

            return combined.map((evt) => (
              <Link
                key={evt.eventID}
                to={
                  localStorage.getItem("token")
                    ? `/user/events/${evt.eventID}`
                    : `/events/${evt.eventID}`
                }
                style={{ textDecoration: "none", color: "inherit" }}
              >
                <Card
                  name={evt.name}
                  startDate={evt.startTime}
                  endDate={evt.endTime}
                  image={evt.photoUrl}
                  orgId={evt.organizationID}
                />
              </Link>
            ));
          })()}
        </div>
      </div>

      <div>
        <h2>Organizations</h2>
        <div style={{ display: "flex", justifyContent: "flex-end" }}>
          <Button
            variant="outlined"
            color="success"
            onClick={() =>
              navigate(
                localStorage.getItem("token")
                  ? "/user/organizations"
                  : "/organizations"
              )
            }
            sx={{ fontSize: "16px", marginTop: "10px", marginBottom: "20px" }}
          >
            view more
          </Button>
        </div>
        <div style={{ display: "flex", gap: "10px", flexWrap: "wrap" }}>
          {org.slice(0, 4).map((each) => (
            <OrgCard
              key={each.organizationID}
              name={each.name}
              image={each.logoUrl}
              to={
                localStorage.getItem("token")
                  ? `/user/organizations/${each.organizationID}`
                  : `/organizations/${each.organizationID}`
              }
            />
          ))}
        </div>
      </div>
    </div>
  );
}
