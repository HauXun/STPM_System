import { IonIcon } from '@ionic/react';
import { Avatar, Box, IconButton, Paper, Stack, Tab, Tabs, Typography } from '@mui/material';
import { darken } from '@mui/material/styles';
import { bookmarkOutline, heartOutline, radioOutline } from 'ionicons/icons';
import { useState } from 'react';
import { useParams } from 'react-router-dom';
import { useTopicDetails } from '~/app/hooks';
import CommentArea from '~/app/modules/comment/presentation/pages/CommentArea';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import MediaCarousel from '~/app/modules/shared/presentation/components/MediaCarousel';
import TopicWorkFlow from './TopicWorkFlow';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}

function a11yProps(index: number) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

export default function TopicPost() {
  const [value, setValue] = useState(0);
  const { topicId } = useParams<{ topicId: string }>();
  const topic = useTopicDetails(topicId || '');

  const handleChange = (event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

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
        {topic?.topicName}
      </Typography>
      <BoxFlexCenter>
        <BoxFlexCenter>
          <Avatar
            alt="Avatar"
            src={topic?.leader.imageUrl || 'https://picsum.photos/300/300'}
            sx={{ width: 50, height: 50, mr: 2 }}
          />
          <Box>
            <Typography className="text-xl font-semibold text-gray-500">
              {topic?.leader.fullName}
            </Typography>
            <Typography className="text-sm" color="var(--primary-green)">
              {topic?.regisDate.toLocaleString()}
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
      <Typography className="text-justify indent-14 text-gray-500">{topic?.description}</Typography>
      <MediaCarousel
        photos={topic?.topicPhotos.map((p) => p.imageUrl)}
        videos={topic?.topicVideos.map((v) => v.videoUrl)}
      />
      <Paper elevation={0} sx={{ px: 2, boxShadow: COMPONENT_SHADOW, borderRadius: 3 }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <Tabs
            value={value}
            onChange={handleChange}
            aria-label="basic tabs example"
            sx={{
              '& .MuiTab-root ': {
                fontWeight: 600,
              },
            }}
          >
            <Tab
              label={
                <Box className="flex items-center text-lg normal-case">
                  <IonIcon className="mr-2 text-sky-600" icon={radioOutline} />
                  Mô tả
                </Box>
              }
              {...a11yProps(0)}
            />
            <Tab label={<Box className="text-lg normal-case">Thành viên</Box>} {...a11yProps(1)} />
          </Tabs>
        </Box>
        <TabPanel value={value} index={0}>
          {topic?.shortDescription}
        </TabPanel>
        <TabPanel value={value} index={1}>
          <TopicWorkFlow topic={topic} sx={{ boxShadow: 'none', p: 0 }} />
        </TabPanel>
      </Paper>
      <CommentArea />
    </Stack>
  );
}
