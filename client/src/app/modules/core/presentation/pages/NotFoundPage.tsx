import { Box, Button, Container, Typography } from '@mui/material';
import Grid from '@mui/material/Grid';

export default function NotFoundPage() {
  return (
    <Box
      sx={{
        mt: '10%',
      }}
    >
      <Container>
        <Grid container spacing={0} alignItems="center" justifyContent="center">
          <Grid item xs={6}>
            <Typography variant="h1">404</Typography>
            <Typography variant="h6">The page you’re looking for doesn’t exist.</Typography>
            <Button variant="contained">Back Home</Button>
          </Grid>
          <Grid item xs={6}>
            <img
              src="https://cdni.iconscout.com/illustration/premium/thumb/404-error-3702359-3119148.png"
              alt="not_found"
            />
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
}
