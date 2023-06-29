import { ArrowBackRounded, ArrowForwardRounded } from '@mui/icons-material/';
import {
  Box,
  Card,
  CardContent,
  CardHeader,
  CardMedia,
  Divider,
  IconButton,
  Paper,
  PaperProps,
  SxProps,
  Theme,
  Typography,
} from '@mui/material';
import { grey } from '@mui/material/colors';
import { alpha, hexToRgb } from '@mui/material/styles';
import { makeStyles } from '@mui/styles';
import { useEffect, useRef } from 'react';
import Slider, { Settings } from 'react-slick';
import 'slick-carousel/slick/slick-theme.css';
import 'slick-carousel/slick/slick.css';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import { centerFlexItems } from '~/app/modules/shared/utils';
import { TopicFilterModel } from '~/app/modules/topic/domain/models/TopicFilterModel';
import { TopicState } from '~/app/modules/topic/infrastructure/store/types';

const useStyles = makeStyles((theme) => ({
  slider: {
    width: '100%',
    height: 'fit-content',
    '& .slick-slide': {
      minWidth: 'auto',
      margin: '0 10px',
    },
    '& .slick-list': {
      height: 275,
      '& .slick-track': {
        height: 'inherit',
      },
    },
    '& .slick-dots': {
      bottom: -50,
      listStyle: 'none',
      '& li': {
        width: 15,
        height: 15,
        borderRadius: 10,
        backgroundColor: alpha(theme.palette.primary.main, 0.1),
        transition: 'all 0.1s ease-in',
        '&:hover': {
          backgroundColor: alpha(theme.palette.primary.main, 0.4),
        },
        '& button': {
          width: 'inherit',
          height: 'inherit',
          padding: 0,
          '&:before': {
            fontSize: 'unset',
            fontFamily: 'unset',
          },
        },
        '&.slick-active': {
          width: 50,
          backgroundColor: theme.palette.primary.main,
        },
      },
    },
  },
  sliderItems: {
    ...({
      position: 'relative',
      width: 'auto',
      scrollSnapAlign: 'center',
      cursor: 'pointer',
      ...centerFlexItems({ direction: 'column', align: 'initial' }),
    } as SxProps<Theme>),
  },
  sliderActionsContainer: {
    position: 'absolute',
    width: '100%',
    height: '100%',
    zIndex: 1,
    pointerEvents: 'none',
    background: `linear-gradient(90deg, ${hexToRgb('#fff')} 0%,
    ${alpha('#fff', 0.9)} 5%,
    rgba(255,255,255,0) 15%,
    rgba(255,255,255,0) 85%,
    ${alpha('#fff', 0.9)} 95%,
    ${hexToRgb('#fff')} 100%)`,
  },
}));

type Props = PaperProps & {
  userId: string;
  topicState: TopicState;
  fetchTopics: (model: TopicFilterModel) => void;
  setFilter: (model: TopicFilterModel) => void;
};

// const images = [
//   'https://images.unsplash.com/photo-1537944434965-cf4679d1a598?auto=format&fit=crop&w=400&h=250&q=60',
//   'https://images.unsplash.com/photo-1538032746644-0212e812a9e7?auto=format&fit=crop&w=400&h=250&q=60',
//   'https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=400&h=250',
//   'https://images.unsplash.com/photo-1512341689857-198e7e2f3ca8?auto=format&fit=crop&w=400&h=250&q=60',
//   'https://images.unsplash.com/photo-1537944434965-cf4679d1a598?auto=format&fit=crop&w=400&h=250&q=60',
//   'https://images.unsplash.com/photo-1538032746644-0212e812a9e7?auto=format&fit=crop&w=400&h=250&q=60',
//   'https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=400&h=250',
//   'https://images.unsplash.com/photo-1512341689857-198e7e2f3ca8?auto=format&fit=crop&w=400&h=250&q=60',
//   'https://www.youtube.com/embed/tgbNymZ7vqY',
// ];

export default function UserTopicJoined({ sx, userId, topicState, fetchTopics, setFilter }: Props) {
  const slider = useRef<Slider>(null);
  const { data, filter } = topicState;
  
  useEffect(() => {
    fetchTopics(filter);
  }, [fetchTopics, filter]);

  useEffect(() => {
    setFilter({ userId: Number(userId) } as TopicFilterModel);
  }, [setFilter, userId]);

  const handleBackClick = (event: React.MouseEvent<HTMLElement>) => {
    slider.current && slider.current.slickPrev();
  };

  const handleForwardClick = (event: React.MouseEvent<HTMLElement>) => {
    slider.current && slider.current.slickNext();
  };

  const classes = useStyles();

  const NonButton = () => null;

  const settings: Settings = {
    dots: true,
    infinite: true,
    speed: 500,
    // autoplay: true,
    // autoplaySpeed: 3000,
    // pauseOnHover: true,
    slidesToShow: 3,
    slidesToScroll: 1,
    prevArrow: <NonButton />,
    nextArrow: <NonButton />,
  };

  return (
    <Paper
      sx={{
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        p: 2,
        borderRadius: 2,
        boxShadow: COMPONENT_SHADOW,
        rowGap: 1.5,
      }}
    >
      <Typography color="var(--primary-green)" className="text-lg font-bold" variant="body1">
        Tham gia
      </Typography>
      <Divider />
      <Box className="mb-16">
        <Box
          sx={{
            position: 'relative',
            mx: 'auto',
          }}
        >
          <BoxFlexCenter className={classes.sliderActionsContainer}>
            <IconButton
              onClick={handleBackClick}
              sx={{
                width: '30px',
                height: '30px',
                cursor: 'pointer',
                pointerEvents: 'auto',
                ml: 2,
              }}
            >
              <ArrowBackRounded />
            </IconButton>
            <IconButton
              onClick={handleForwardClick}
              sx={{
                width: '30px',
                height: '30px',
                cursor: 'pointer',
                pointerEvents: 'auto',
                mr: 2,
              }}
            >
              <ArrowForwardRounded />
            </IconButton>
          </BoxFlexCenter>
          <Slider {...settings} className={classes.slider} ref={slider}>
            {data &&
              data.length > 0 &&
              data.map((topic, i) => (
                <Card
                  key={i}
                  elevation={0}
                  className={classes.sliderItems}
                  sx={{ '&>*': { p: 0 } }}
                >
                  <CardMedia
                    className="rounded-lg"
                    component="img"
                    height={150}
                    image={
                      topic.topicPhotos[0]
                        ? topic.topicPhotos[0].imageUrl
                        : 'https://upload.wikimedia.org/wikipedia/commons/0/00/Nothing_Logo.webp'
                    }
                    alt="Paella dish"
                  />
                  <CardHeader
                    sx={{
                      py: 0,
                      mt: 1.5,
                      '& .MuiTypography-root': {
                        fontSize: '1.1rem',
                        fontWeight: 500,
                        color: grey[600],
                      },
                    }}
                    title={`# ${topic.topicRank.rankName}`}
                  />
                  <CardContent
                    className="py-0"
                    sx={{
                      '& .MuiTypography-root': {
                        overflow: 'hidden',
                        display: '-webkit-box',
                        WebkitBoxOrient: 'vertical',
                      },
                    }}
                  >
                    <Typography
                      className="font-bold"
                      variant="h6"
                      sx={{
                        WebkitLineClamp: 1,
                      }}
                    >
                      {topic.topicName}
                    </Typography>
                    <Typography
                      className="font-thin text-gray-600"
                      variant="subtitle2"
                      sx={{
                        WebkitLineClamp: 2,
                      }}
                    >
                      {topic.shortDescription}
                    </Typography>
                  </CardContent>
                </Card>
              ))}
          </Slider>
        </Box>
      </Box>
    </Paper>
  );
}
