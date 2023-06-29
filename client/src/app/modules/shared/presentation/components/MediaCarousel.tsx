import { ArrowBackRounded, ArrowForwardRounded } from '@mui/icons-material/';
import { Box, IconButton } from '@mui/material';
import { useEffect, useRef, useState } from 'react';
import { COMPONENT_SHADOW } from '../../constants';
import { hexToRgb, alpha } from '@mui/material/styles';
import Slider, { Settings } from 'react-slick';
import { makeStyles } from '@mui/styles';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';

const useStyles = makeStyles((theme) => ({
  slider: {
    width: '100%',
    height: 'fit-content',
    '& .slick-slide': {
      minWidth: 180,
      margin: '0 10px',
      '&>div': {
        width: '100%',
        height: '100%',
      },
    },
    '& .slick-list': {
      height: 120,
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
}));

type Media = { path: string; isVideo: boolean };

interface Props {
  photos?: string[];
  videos?: string[];
}

// const medias: Media[] = [
//   {
//     path: 'https://images.unsplash.com/photo-1537944434965-cf4679d1a598?auto=format&fit=crop&w=400&h=250&q=60',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1538032746644-0212e812a9e7?auto=format&fit=crop&w=400&h=250&q=60',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=400&h=250',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1512341689857-198e7e2f3ca8?auto=format&fit=crop&w=400&h=250&q=60',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1537944434965-cf4679d1a598?auto=format&fit=crop&w=400&h=250&q=60',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1538032746644-0212e812a9e7?auto=format&fit=crop&w=400&h=250&q=60',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=400&h=250',
//     isVideo: false,
//   },
//   {
//     path: 'https://images.unsplash.com/photo-1512341689857-198e7e2f3ca8?auto=format&fit=crop&w=400&h=250&q=60',
//     isVideo: false,
//   },
//   {
//     path: 'https://www.youtube.com/embed/tgbNymZ7vqY',
//     isVideo: true,
//   },
// ];

export default function MediaCarousel({ photos, videos }: Props) {
  const [media, setMedia] = useState<Media>();
  const [medias, setMedias] = useState<Media[]>([]);
  const slider = useRef<Slider>(null);

  useEffect(() => {
    let newMedias: Media[] = [];

    if (photos && photos.length > 0) {
      const photoMedias = photos.map((photo) => ({ path: photo, isVideo: false } as Media));
      newMedias = [...newMedias, ...photoMedias];
    }

    if (videos && videos.length > 0) {
      const videoMedias = videos.map((video) => ({ path: video, isVideo: true } as Media));
      newMedias = [...newMedias, ...videoMedias];
    }

    setMedias(newMedias);
  }, [photos, videos]);

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
    slidesToShow: 5,
    slidesToScroll: 1,
    prevArrow: <NonButton />,
    nextArrow: <NonButton />,
  };

  return (
    <Box className="mb-16">
      <Box
        sx={{
          width: 800,
          height: 500,
          borderRadius: '10px',
          overflow: 'hidden',
          mb: 2,
          mx: 'auto',
          boxShadow: COMPONENT_SHADOW,
        }}
      >
        {media && media.isVideo ? (
          <iframe
            title="video"
            style={{
              width: 'inherit',
              height: 'inherit',
              border: 0,
            }}
            src={
              media?.path || "https://www.youtube.com/embed/mw9WcQo6aIY"
            }
          ></iframe>
        ) : (
          <Box
            component="img"
            sx={{
              width: 'inherit',
              objectFit: 'contain',
            }}
            src={
              media?.path ?? 'https://upload.wikimedia.org/wikipedia/commons/0/00/Nothing_Logo.webp'
            }
          />
        )}
      </Box>
      {medias && medias.length > 0 && (
        <Box
          sx={{
            width: 180 * 5 - 180 / 2 - 10,
            position: 'relative',
            mx: 'auto',
          }}
        >
          <BoxFlexCenter
            sx={{
              position: 'absolute',
              width: '100%',
              height: '100%',
              zIndex: 1,
              pointerEvents: 'none',
              background: (theme) => `linear-gradient(90deg, ${hexToRgb(
                theme.palette.background.default
              )} 0%,
                ${alpha(theme.palette.background.default, 0.9)} 5%,
                rgba(255,255,255,0) 15%,
                rgba(255,255,255,0) 85%,
                ${alpha(theme.palette.background.default, 0.9)} 95%,
                ${hexToRgb(theme.palette.background.default)} 100%)`,
            }}
          >
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
            {medias.map((media, index) => {
              return media.isVideo ? (
                <Box
                  key={index}
                  sx={{
                    cursor: 'pointer',
                    overflow: 'hidden',
                    borderRadius: '6px',
                    height: '100%',
                  }}
                  onClick={(e) => setMedia(media)}
                >
                  <iframe
                    title="video"
                    style={{
                      width: '100%',
                      pointerEvents: 'none',
                      border: 0,
                    }}
                    src={media.path}
                  ></iframe>
                </Box>
              ) : (
                <Box
                  component="img"
                  key={index}
                  sx={{
                    scrollSnapAlign: 'center',
                    cursor: 'pointer',
                    borderRadius: '6px',
                    height: '100%',
                  }}
                  src={media.path}
                  onClick={(e) => setMedia(media)}
                />
              );
            })}
          </Slider>
        </Box>
      )}
    </Box>
  );
}
