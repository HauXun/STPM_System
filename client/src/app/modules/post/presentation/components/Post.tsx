import { IonIcon } from '@ionic/react';
import { Avatar, Box, IconButton, Stack, Typography } from '@mui/material';
import { darken } from '@mui/material/styles';
import { bookmarkOutline, heartOutline } from 'ionicons/icons';
import { usePostDetails } from '~/app/hooks';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import MediaCarousel from '~/app/modules/shared/presentation/components/MediaCarousel';

type PostProps = {
  postId: string;
};

export default function Post({ postId }: PostProps) {
  const post = usePostDetails(postId);
  
  return (
    <Stack spacing={4}>
      <Typography
        className="font-bold"
        sx={{
          color: (theme) => darken(theme.palette.primary.main, 0.3),
        }}
        variant="h5"
        component="h1"
      >
        {post?.title}
      </Typography>
      <BoxFlexCenter>
        <BoxFlexCenter>
          <Avatar
            alt="Avatar"
            src={post?.user.imageUrl || 'https://picsum.photos/300/300'}
            sx={{ width: 50, height: 50, mr: 2 }}
          />
          <Box>
            <Typography className="text-xl font-semibold text-gray-500">
              {post?.user.fullName}
            </Typography>
            <Typography className="text-sm" color="var(--primary-green)">
              {post && new Date(post.postedDate).toLocaleString()}
            </Typography>
          </Box>
        </BoxFlexCenter>
        <BoxFlexCenter>
          <IconButton>
            <IonIcon icon={heartOutline} />
          </IconButton>
          <IconButton>
            <IonIcon icon={bookmarkOutline} />
          </IconButton>
        </BoxFlexCenter>
      </BoxFlexCenter>
      <Typography className="text-justify indent-14 text-gray-500">
        {post?.shortDescription}
      </Typography>
      <MediaCarousel
        photos={post?.postPhotos.map((p) => p.imageUrl)}
        videos={post?.postVideos.map((v) => v.videoUrl)}
      />
      <Typography className="mb-20 text-justify indent-14 text-gray-500">
        {post?.description}
      </Typography>
    </Stack>
  );
}
