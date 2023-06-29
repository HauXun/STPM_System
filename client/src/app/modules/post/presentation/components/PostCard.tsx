import { IonIcon } from '@ionic/react';
import { EastRounded } from '@mui/icons-material';
import {
  Avatar,
  Box,
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  CardMedia,
  CardProps,
  Divider,
  Stack,
  SxProps,
  Theme,
  Typography,
} from '@mui/material';
import { grey, red } from '@mui/material/colors';
import { eyeOutline, heartOutline } from 'ionicons/icons';
import { Link, useLocation } from 'react-router-dom';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';
import { UserRoutes } from '~/app/modules/user/presentation/routes';
import { Post } from '../../domain/models/Post';

type PostCardProps = CardProps & {
  post: Post;
};

export default function PostCard({ sx, post }: PostCardProps) {
  const { pathname } = useLocation();
  const { id, title, shortDescription, viewCount, postPhotos, user } = post;

  return (
    <Card
      elevation={0}
      sx={{
        ...({
          position: 'relative',
          width: 'auto',
          borderRadius: 2,
          p: 3,
          pb: 0,
          boxShadow: COMPONENT_SHADOW,
          ...centerFlexItems({ direction: 'column', align: 'initial' }),
          ...sx,
        } as SxProps<Theme>),
      }}
    >
      <Box component={Link} to={`${pathname}/${id || ''}`}>
        <Box height={230}>
          <CardMedia
            className="rounded-lg"
            component="img"
            height="100%"
            image={
              postPhotos[0]
                ? postPhotos[0].imageUrl
                : 'https://q-dance-network-images.akamaized.net/41347/1666098985-0001_20220625165639_kevin_453a5245-edit.jpg?auto=compress&fit=max?w=1200'
            }
            alt="Paella dish"
          />
        </Box>
      </Box>
      <CardHeader
        sx={{
          '& .MuiTypography-root': {
            fontSize: '1.1rem',
            fontWeight: 500,
            color: grey[600],
          },
        }}
        component={Link}
        to={`/${UserRoutes.USERS}/${user.id || ''}`}
        avatar={
          <Avatar
            sx={{ bgcolor: red[500] }}
            src={user.imageUrl || 'https://picsum.photos/300/300'}
            aria-label="recipe"
          />
        }
        title={user.fullName}
      />
      <Box component={Link} to={`${pathname}/${id || ''}`}>
        <CardContent className="py-0">
          <Typography className="font-bold" variant="h6">
            {title}
          </Typography>
          <Typography
            variant="body2"
            color="text.primary"
            sx={{
              WebkitLineClamp: 3,
              overflow: 'hidden',
              display: '-webkit-box',
              WebkitBoxOrient: 'vertical',
            }}
          >
            {shortDescription}
          </Typography>
        </CardContent>
      </Box>
      <Box>
        <Divider className="mt-5" />
        <CardActions disableSpacing>
          <BoxFlexCenter sx={{ width: '100%' }}>
            <Stack direction="row" spacing={3} className="text-xl">
              <BoxFlexCenter>
                <IonIcon icon={eyeOutline} />
                <Typography className="ml-1 text-base" variant="caption">
                  {viewCount}
                </Typography>
              </BoxFlexCenter>
              <BoxFlexCenter>
                <IonIcon icon={heartOutline} />
                <Typography className="ml-1 text-base" variant="caption">
                  704
                </Typography>
              </BoxFlexCenter>
            </Stack>
            <Button
              component={Link}
              to={`${pathname}/${id || ''}`}
              className="rounded-3xl"
              variant="contained"
              sx={{ py: 0.5, px: 4 }}
            >
              <EastRounded />
            </Button>
          </BoxFlexCenter>
        </CardActions>
      </Box>
    </Card>
  );
}
