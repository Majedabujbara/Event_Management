import React, { useState, useEffect } from "react";
import {
  InputAdornment,
  TextField,
  Paper,
  List,
  ListItem,
  ListItemText,
  Divider,
  ListSubheader,
  Fade,
  Typography,
  IconButton,
  CircularProgress
} from "@mui/material";
import {
  Search as SearchIcon,
  Close as CloseIcon,
  Event as EventIcon,
  Article as NewsIcon,
  Groups as OrgIcon
} from "@mui/icons-material";
import { useNavigate } from "react-router-dom";
import { searchAll } from "../utility/apiGetCalls";
import { styled } from "@mui/system";

const StyledPaper = styled(Paper)(({ theme }) => ({
  position: 'absolute',
  width: '100%',
  marginTop: '4px',
  maxHeight: 'min(400px, 60vh)',
  overflowY: 'auto',
  borderRadius: '8px',
  background: 'rgba(255, 255, 255, 0.98)',
  backdropFilter: 'blur(12px)',
  boxShadow: '0 4px 12px rgba(0,0,0,0.1)',
  zIndex: 1000,
  '&::-webkit-scrollbar': {
    width: '4px',
  },
  '&::-webkit-scrollbar-thumb': {
    backgroundColor: 'rgba(0,0,0,0.1)',
    borderRadius: '2px',
  },
}));

const SearchBar = () => {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState({ events: [], news: [], orgs: [] });
  const [showDropdown, setShowDropdown] = useState(false);
  const [isFocused, setIsFocused] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const search = async () => {
      if (query.trim().length > 1) {
        setIsLoading(true);
        try {
          const data = await searchAll(query);
          setResults(data);
          setShowDropdown(true);
        } catch (error) {
          console.error("Search error:", error);
        } finally {
          setIsLoading(false);
        }
      } else {
        setShowDropdown(false);
        setIsLoading(false);
      }
    };

    const timeout = setTimeout(search, 350);
    return () => clearTimeout(timeout);
  }, [query]);

  const handleClear = () => {
    setQuery("");
    setShowDropdown(false);
  };

  const handleClick = (type, id) => {
    if (type === "event") navigate(localStorage.getItem('token') ? `/user/events/${id}` : `/events/${id}`);
    else if (type === "news") navigate(localStorage.getItem('token') ? `/user/news/${id}` : `/news/${id}`);
    else if (type === "org") navigate(localStorage.getItem('token') ? `/user/organizations/${id}` : `/organizations/${id}`);
    setShowDropdown(false);
  };

  const getCategoryIcon = (category) => {
    switch (category) {
      case "Events":
        return <EventIcon fontSize="small" color="primary" sx={{ mr: 1 }} />;
      case "News":
        return <NewsIcon fontSize="small" color="secondary" sx={{ mr: 1 }} />;
      case "Organizations":
        return <OrgIcon fontSize="small" color="success" sx={{ mr: 1 }} />;
      default:
        return null;
    }
  };

  return (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      width: 'auto',
      minWidth: '250px',
      flexGrow: 1,
      maxWidth: '500px',
      margin: '0 16px',
      position: 'relative',
    }}>
      <div style={{
        width: '100%',
        position: 'relative',
      }}>
        <TextField
          fullWidth
          variant="outlined"
          placeholder="Search events, news, organizations..."
          value={query}
          size="small"
          onChange={(e) => setQuery(e.target.value)}
          onBlur={() => {
            setTimeout(() => setShowDropdown(false), 200);
            setIsFocused(false);
          }}
          onFocus={() => {
            if (query) setShowDropdown(true);
            setIsFocused(true);
          }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon style={{ 
                  color: isFocused ? '#3f51b5' : '#757575',
                  fontSize: '20px'
                }} />
              </InputAdornment>
            ),
            endAdornment: (
              <InputAdornment position="end">
                {isLoading ? (
                  <CircularProgress size={16} thickness={5} />
                ) : query ? (
                  <IconButton
                    size="small"
                    onClick={handleClear}
                    edge="end"
                    sx={{ p: '2px' }}
                  >
                    <CloseIcon fontSize="small" />
                  </IconButton>
                ) : null}
              </InputAdornment>
            ),
            style: {
              borderRadius: '20px',
              backgroundColor: 'rgba(255, 255, 255, 0.95)',
              boxShadow: isFocused
                ? '0 2px 8px rgba(63, 81, 181, 0.1)'
                : '0 1px 4px rgba(0,0,0,0.05)',
              height: '40px',
              paddingLeft: '12px',
              paddingRight: '6px',
            }
          }}
          sx={{
            '& .MuiOutlinedInput-root': {
              '& fieldset': {
                borderColor: isFocused ? '#3f51b5' : 'rgba(0,0,0,0.08)',
                borderWidth: isFocused ? '1.5px' : '1px',
              },
              '&:hover fieldset': {
                borderColor: '#3f51b5',
              },
              '&.Mui-focused fieldset': {
                borderColor: '#3f51b5',
              },
            },
            '& .MuiInputBase-input': {
              padding: '8px 6px',
              fontSize: '0.9rem',
            }
          }}
        />

        <Fade in={showDropdown && (results.events.length > 0 || results.news.length > 0 || results.orgs.length > 0)}>
          <StyledPaper elevation={4}>
            {results.events.length > 0 && (
              <>
                <List subheader={
                  <ListSubheader sx={{
                    bgcolor: 'transparent',
                    display: 'flex',
                    alignItems: 'center',
                    py: 1,
                    px: 2,
                    color: 'text.primary',
                    fontWeight: 600,
                    fontSize: '0.875rem'
                  }}>
                    {getCategoryIcon("Events")}
                    <Typography variant="subtitle2">Events</Typography>
                  </ListSubheader>
                }>
                  {results.events.map((event) => (
                    <ListItem
                      button
                      key={event.eventID}
                      onClick={() => handleClick("event", event.eventID)}
                      sx={{
                        '&:hover': {
                          backgroundColor: 'rgba(63, 81, 181, 0.05)'
                        },
                        py: 1,
                        px: 2,
                      }}
                    >
                      <ListItemText 
                        primary={event.name}
                        primaryTypographyProps={{ fontSize: '0.875rem' }}
                        secondary={event.date ? new Date(event.date).toLocaleDateString() : ''}
                        secondaryTypographyProps={{ fontSize: '0.75rem' }}
                      />
                    </ListItem>
                  ))}
                </List>
                <Divider sx={{ my: 0.5 }} />
              </>
            )}
            
            {results.news.length > 0 && (
              <>
                <List subheader={
                  <ListSubheader sx={{
                    bgcolor: 'transparent',
                    display: 'flex',
                    alignItems: 'center',
                    py: 1,
                    px: 2,
                    color: 'text.primary',
                    fontWeight: 600,
                    fontSize: '0.875rem'
                  }}>
                    {getCategoryIcon("News")}
                    <Typography variant="subtitle2">News</Typography>
                  </ListSubheader>
                }>
                  {results.news.map((news) => (
                    <ListItem
                      button
                      key={news.id}
                      onClick={() => handleClick("news", news.id)}
                      sx={{
                        '&:hover': {
                          backgroundColor: 'rgba(63, 81, 181, 0.05)'
                        },
                        py: 1,
                        px: 2,
                      }}
                    >
                      <ListItemText 
                        primary={news.title}
                        primaryTypographyProps={{ fontSize: '0.875rem' }}
                        secondary={news.date ? new Date(news.date).toLocaleDateString() : ''}
                        secondaryTypographyProps={{ fontSize: '0.75rem' }}
                      />
                    </ListItem>
                  ))}
                </List>
                <Divider sx={{ my: 0.5 }} />
              </>
            )}
            
            {results.orgs.length > 0 && (
              <List subheader={
                <ListSubheader sx={{
                  bgcolor: 'transparent',
                  display: 'flex',
                  alignItems: 'center',
                  py: 1,
                  px: 2,
                  color: 'text.primary',
                  fontWeight: 600,
                  fontSize: '0.875rem'
                }}>
                  {getCategoryIcon("Organizations")}
                  <Typography variant="subtitle2">Organizations</Typography>
                </ListSubheader>
              }>
                {results.orgs.map((org) => (
                  <ListItem
                    button
                    key={org.organizationID}
                    onClick={() => handleClick("org", org.organizationID)}
                    sx={{
                      '&:hover': {
                        backgroundColor: 'rgba(63, 81, 181, 0.05)'
                      },
                      py: 1,
                      px: 2,
                    }}
                  >
                    <ListItemText 
                      primary={org.name}
                      primaryTypographyProps={{ fontSize: '0.875rem' }}
                      secondary={org.category || ''}
                      secondaryTypographyProps={{ fontSize: '0.75rem' }}
                    />
                  </ListItem>
                ))}
              </List>
            )}
          </StyledPaper>
        </Fade>
        
        {showDropdown && !isLoading && 
         results.events.length === 0 && 
         results.news.length === 0 && 
         results.orgs.length === 0 && 
         query.length > 1 && (
          <Fade in={true}>
            <StyledPaper>
              <Typography variant="body2" sx={{ p: 2, textAlign: 'center', color: 'text.secondary' }}>
                No results found for "{query}"
              </Typography>
            </StyledPaper>
          </Fade>
        )}
      </div>
    </div>
  );
};

export default SearchBar;