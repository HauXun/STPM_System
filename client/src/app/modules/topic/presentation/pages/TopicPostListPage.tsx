import { Box, ListItemIcon, MenuItem, Select, SxProps, Theme, Typography } from '@mui/material';
import { ChangeEvent, useEffect } from 'react';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';
import { COMPONENT_SHADOW } from '~/app/modules/shared/constants';
import SearchInput from '~/app/modules/shared/presentation/components/SearchInput';
import { centerFlexItems } from '~/app/modules/shared/utils';
import { useGlobalContext } from '~/main/app';
import RankAward_Type1 from '~/main/assets/RankAward_Type1.png';
import RankAward_Type2 from '~/main/assets/RankAward_Type2.png';
import TopicPostListContainer from '../containers/TopicPostListContainer';

export default function TopicPostListPage() {
  const { setTitle } = useGlobalContext();

  useEffect(() => {
    setTitle('Danh sách đề tài');
  }, [setTitle]);

  const options = [
    {
      value: '1',
      label: 'Hạng mục 1',
      icon: (
        <Box
          component="img"
          sx={{
            width: 30,
            m: 1,
            ml: 2,
            objectFit: 'contain',
          }}
          src={RankAward_Type1}
        />
      ),
    },
    {
      value: '2',
      label: 'Hạng mục 2',
      icon: (
        <Box
          component="img"
          sx={{
            width: 30,
            m: 1,
            ml: 2,
            objectFit: 'contain',
          }}
          src={RankAward_Type2}
        />
      ),
    },
    // Add more options as needed
  ];

  return (
    <Box
      sx={{
        display: 'grid',
        gridTemplateColumns: '900px 1fr',
        gridTemplateRows: 'auto 1fr auto',
        gridTemplateAreas: `
          'h1 h1' 'h2 h3'
        `,
        gap: 5,
      }}
    >
      <BoxFlexCenter sx={{ gridArea: 'h1', width: 900 }}>
        <SearchInput
          placeholder="Tìm kiếm đề tài"
          onChange={(event: ChangeEvent<HTMLInputElement>) => console.log(event.target.value)}
        />
        <Select
          displayEmpty
          IconComponent={() => (
            <Box
              component="img"
              sx={{
                width: 30,
                m: 1,
                objectFit: 'contain',
              }}
              src={RankAward_Type1}
            />
          )}
          renderValue={() => (
            <Typography className="text-base leading-4 text-gray-500" variant="body2">
              Hạng mục 1
            </Typography>
          )}
          sx={{
            ...({
              borderRadius: 2,
              backgroundColor: 'white',
              boxShadow: COMPONENT_SHADOW,
              '& .MuiSelect-select': {
                py: 0,
                ...centerFlexItems({}),
              },
            } as SxProps<Theme>),
          }}
        >
          {options.map((option) => (
            <MenuItem key={option.value} value={option.value} sx={{}}>
              {option.label}
              <ListItemIcon>{option.icon}</ListItemIcon>
            </MenuItem>
          ))}
        </Select>
      </BoxFlexCenter>
      <BoxFlexCenter direction='column' sx={{ gridArea: 'h2', rowGap: 6  }}>
        <TopicPostListContainer />
      </BoxFlexCenter>
      <Box sx={{ gridArea: 'h3' }}>Add something else right here.</Box>
    </Box>
  );
}
