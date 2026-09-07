import { Link, useNavigate } from "react-router-dom";
import Card from "../components/Card";
import { useEffect, useState } from "react";
import {
  Snackbar,
  Alert,
  Button,
  Typography,
  Pagination,
  Box,
} from "@mui/material";
import { EventNote, Add } from "@mui/icons-material";
import { getEvents } from "../utility/apiGetCalls";

function sortEventsByTime(events) {
  const now = new Date();

  const upcoming = [];
  const ongoing = [];
  const ended = [];

  events.forEach((event) => {
    const start = new Date(event.startTime);
    const end = new Date(event.endTime);

    if (start > now) {
      upcoming.push(event);
    } else if (start <= now && end >= now) {
      ongoing.push(event);
    } else {
      ended.push(event);
    }
  });

  return [...ongoing, ...upcoming, ...ended];
}

export default function Events() {
  const navigate = useNavigate();
  const [events, setEvents] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const eventsPerPage = 9;

  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarUnreg, setSnackbarUnreg] = useState(false);

  const handleCloseRegistrationSnackbar = () => {
    setSnackbarOpen(false);
  };

  const handleCloseUnregistrationSnackbar = () => {
    setSnackbarUnreg(false);
  };

  useEffect(() => {
    async function fetchAndSetEvents() {
      const data = await getEvents();
      if (data) {
        const sortedEvents = sortEventsByTime(data);
        setEvents(sortedEvents);
        setCurrentPage(1);
      }
    }

    fetchAndSetEvents();

    if (localStorage.getItem("registrationSuccess") === "true") {
      setSnackbarOpen(true);
      localStorage.removeItem("registrationSuccess");
    }

    if (localStorage.getItem("unregistrationSuccess") === "true") {
      setSnackbarUnreg(true);
      localStorage.removeItem("unregistrationSuccess");
    }
  }, []);

  const totalPages = Math.ceil(events.length / eventsPerPage);
  const indexOfLastEvent = currentPage * eventsPerPage;
  const indexOfFirstEvent = indexOfLastEvent - eventsPerPage;
  const currentEvents = events.slice(indexOfFirstEvent, indexOfLastEvent);

  const handlePageChange = (event, value) => {
    setCurrentPage(value);
  };

  return (
    <div className="events-page">
      <div className="events-header">
        <Typography variant="h4" component="h1" className="events-title">
          <EventNote className="title-icon" />
          University Events
        </Typography>
        {localStorage.getItem("email") === "admin@example.com" && (
          <Button
            variant="contained"
            color="success"
            startIcon={<Add />}
            onClick={() => navigate("new")}
            className="add-event-btn"
          >
            Add Event
          </Button>
        )}
      </div>

      {events.length === 0 ? (
        <div className="no-events">
          <Typography variant="h6">No upcoming events</Typography>
          <Typography variant="body1">
            Check back later for new events!
          </Typography>
        </div>
      ) : (
        <>
          <div className="event-grid">
            {currentEvents.map((evt) => (
              <Link
                key={evt.eventID}
                to={
                  localStorage.getItem("token")
                    ? `/user/events/${evt.eventID}`
                    : `/events/${evt.eventID}`
                }
                className="event-card-link"
              >
                <Card
                  name={evt.name}
                  startDate={evt.startTime}
                  endDate={evt.endTime}
                  orgId={evt.organizationID}
                />
              </Link>
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

      <Snackbar
        open={snackbarOpen}
        autoHideDuration={3000}
        onClose={handleCloseRegistrationSnackbar}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
      >
        <Alert
          onClose={handleCloseRegistrationSnackbar}
          severity="success"
          variant="filled"
        >
          Registration successful!
        </Alert>
      </Snackbar>

      <Snackbar
        open={snackbarUnreg}
        autoHideDuration={3000}
        onClose={handleCloseUnregistrationSnackbar}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
      >
        <Alert
          onClose={handleCloseUnregistrationSnackbar}
          severity="success"
          variant="filled"
        >
          Unregistration successful!
        </Alert>
      </Snackbar>
    </div>
  );
}
